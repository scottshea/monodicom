#region License

// Copyright (c) 2010, ClearCanvas Inc.
// All rights reserved.
//
// Redistribution and use in source and binary forms, with or without modification, 
// are permitted provided that the following conditions are met:
//
//    * Redistributions of source code must retain the above copyright notice, 
//      this list of conditions and the following disclaimer.
//    * Redistributions in binary form must reproduce the above copyright notice, 
//      this list of conditions and the following disclaimer in the documentation 
//      and/or other materials provided with the distribution.
//    * Neither the name of ClearCanvas Inc. nor the names of its contributors 
//      may be used to endorse or promote products derived from this software without 
//      specific prior written permission.
//
// THIS SOFTWARE IS PROVIDED BY THE COPYRIGHT HOLDERS AND CONTRIBUTORS "AS IS" 
// AND ANY EXPRESS OR IMPLIED WARRANTIES, INCLUDING, BUT NOT LIMITED TO, 
// THE IMPLIED WARRANTIES OF MERCHANTABILITY AND FITNESS FOR A PARTICULAR 
// PURPOSE ARE DISCLAIMED. IN NO EVENT SHALL THE COPYRIGHT OWNER OR 
// CONTRIBUTORS BE LIABLE FOR ANY DIRECT, INDIRECT, INCIDENTAL, SPECIAL, EXEMPLARY, 
// OR CONSEQUENTIAL DAMAGES (INCLUDING, BUT NOT LIMITED TO, PROCUREMENT OF SUBSTITUTE 
// GOODS OR SERVICES; LOSS OF USE, DATA, OR PROFITS; OR BUSINESS INTERRUPTION) 
// HOWEVER CAUSED AND ON ANY THEORY OF LIABILITY, WHETHER IN CONTRACT, 
// STRICT LIABILITY, OR TORT (INCLUDING NEGLIGENCE OR OTHERWISE) ARISING IN 
// ANY WAY OUT OF THE USE OF THIS SOFTWARE, EVEN IF ADVISED OF THE POSSIBILITY 
// OF SUCH DAMAGE.

#endregion

using System;

namespace ClearCanvas.Common.Utilities
{
    /// <summary>
    /// Computes the difference between two strings.
    /// </summary>
    /// <remarks>
    /// <para>
	/// The speed and memory requirements are O(n2) for this algorithm, 
	/// so it should not be used on very long strings.
	/// 
	/// Adapted from an algorithm presented here in Javascript:
	/// http://www.csse.monash.edu.au/~lloyd/tildeAlgDS/Dynamic/Edit/
	/// </para>
	/// <para>
    /// The <see cref="AlignedLeft"/> and  <see cref="AlignedRight"/> properties return versions
    /// of the left and right strings that are as closely aligned as possible on a character by
    /// character basis.  '-' characters are inserted into both strings at specific points so as to
    /// produce the closest possible alignment, such that <code>AlignedLeft.Length == AlignedRight.Length</code>. 
    /// The <see cref="DiffMask"/> property is a string of the same length that contains a '|' character
    /// where the aligned strings match and a space where they don't, e.g.
    /// <code>DiffMask[i] = (AlignedLeft[i] == AlignedRight[i]) ? '|' : ' '</code>
	/// </para>
    /// </remarks>
    public class StringDiff
    {
		/// <summary>
		/// Computes the difference between two strings, <paramref name="left"/> 
		/// and <paramref name="right"/>, respectively.
		/// </summary>
		/// <param name="left">The left-hand string.</param>
		/// <param name="right">The right-hand string.</param>
		/// <param name="ignoreCase">Specifies whether or not to ignore character case.</param>
		/// <returns>A <see cref="StringDiff"/> object containing the results.</returns>
        public static StringDiff Compute(string left, string right, bool ignoreCase)
        {
            if (left == right)
            {
                // nop
                return new StringDiff(left, right, new string('|', left.Length));
            }

            string[] result = ComputeDiff(left, right, ignoreCase);
            return new StringDiff(result[0], result[2], result[1]);
        }


        private string _alignedLeft;
        private string _alignedRight;
        private string _diffMask;

        private StringDiff(string alignedLeft, string alignedRight, string diffMask)
        {
            _alignedLeft = alignedLeft;
            _alignedRight = alignedRight;
            _diffMask = diffMask;
        }

		/// <summary>
		/// Returns the left string (input into <see cref="StringDiff.Compute"/>),
		/// but modified to be aligned with the right string.
		/// </summary>
        public string AlignedLeft
        {
            get { return _alignedLeft; }
        }

		/// <summary>
		/// Returns the right string (input into <see cref="StringDiff.Compute"/>),
		/// but modified to be aligned with the left string.
		/// </summary>
		public string AlignedRight
        {
            get { return _alignedRight; }
        }

		/// <summary>
		/// Returns a diff mask, each character in which indicates whether or not
		/// there is a difference at that position between <see cref="AlignedLeft"/>
		/// and <see cref="AlignedRight"/> (space characters indicate no difference,
		/// '|' characters indicate a difference).
		/// </summary>
        public string DiffMask
        {
            get { return _diffMask; }
        }

        private static string[] ComputeDiff(string s1, string s2, bool ignoreCase)
        {
            int[,] m = new int[s1.Length + 1, s2.Length + 1];

            m[0, 0] = 0; // boundary conditions

            for (int j = 1; j <= s2.Length; j++)
                m[0, j] = m[0, j - 1] - 0 + 1; // boundary conditions

            for (int i = 1; i <= s1.Length; i++)                            // outer loop
            {
                m[i, 0] = m[i - 1, 0] - 0 + 1; // boundary conditions

                for (int j = 1; j <= s2.Length; j++)                         // inner loop
                {
                    int diag = m[i - 1, j - 1];
                    if (!CompareEqual(s1[i - 1], s2[j - 1], ignoreCase)) diag++;

                    m[i, j] = Math.Min(diag,               // match or change
                           Math.Min(m[i - 1, j] - 0 + 1,    // deletion
                                     m[i, j - 1] - 0 + 1)); // insertion
                }//for j
            }//for i

            return traceBack("", "", "", m, s1.Length, s2.Length, s1, s2, ignoreCase);
        }

        private static string[] traceBack(string row1, string row2, string row3, int[,] m, int i, int j, string s1, string s2, bool ignoreCase)
        {
            // recover the alignment of s1 and s2
            if (i > 0 && j > 0)
            {
                int diag = m[i - 1, j - 1];
                char diagCh = '|';

                if (!CompareEqual(s1[i - 1], s2[j - 1], ignoreCase)) { diag++; diagCh = ' '; }

                if (m[i, j] == diag) //LAllison comp sci monash uni au
                    return traceBack(s1[i - 1] + row1, diagCh + row2, s2[j - 1] + row3,
                              m, i - 1, j - 1, s1, s2, ignoreCase);    // change or match
                else if (m[i, j] == m[i - 1, j] - 0 + 1) // delete
                    return traceBack(s1[i - 1] + row1, ' ' + row2, '-' + row3,
                              m, i - 1, j, s1, s2, ignoreCase);
                else
                    return traceBack('-' + row1, ' ' + row2, s2[j - 1] + row3,
                              m, i, j - 1, s1, s2, ignoreCase);      // insertion
            }
            else if (i > 0)
                return traceBack(s1[i - 1] + row1, ' ' + row2, '-' + row3, m, i - 1, j, s1, s2, ignoreCase);
            else if (j > 0)
                return traceBack('-' + row1, ' ' + row2, s2[j - 1] + row3, m, i, j - 1, s1, s2, ignoreCase);
            else // i==0 and j==0
            {
                return new string[] { row1, row2, row3 };
            }
        }//traceBack

        private static bool CompareEqual(char c1, char c2, bool ignoreCase)
        {
            return c1 == c2 || (ignoreCase && char.ToLower(c1) == char.ToLower(c2));
        }
    }
}
