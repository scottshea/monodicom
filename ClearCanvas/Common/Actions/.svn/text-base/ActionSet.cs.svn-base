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
using System.Collections.Generic;
using ClearCanvas.Common.Specifications;

namespace ClearCanvas.Common.Actions
{
	/// <summary>
	/// A class used to manage and execute a set of <see cref="Action{T}"/> instances.
	/// </summary>
	/// <typeparam name="T">A context used by the <see cref="Action{T}"/> instances.</typeparam>
    public class ActionSet<T> : IActionSet<T>
    {
        private readonly IList<IActionItem<T>> _actionList;

		/// <summary>
		/// Constructor.
		/// </summary>
		/// <param name="list">The list of actions in the set.</param>
        public ActionSet(IList<IActionItem<T>> list)
        {
            _actionList = list;
        }

		/// <summary>
		/// Execute the actions associated with the set.
		/// </summary>
		/// <param name="context">The context used by the <see cref="Action{T}"/> instances in the set.</param>
		/// <returns>A <see cref="TestResult"/> instance telling the result of executing the actions.</returns>
        public TestResult Execute(T context)
        {
            List<TestResultReason> resultList = new List<TestResultReason>();

            foreach (IActionItem<T> item in _actionList)
            {
                try
                {
                    bool tempResult = item.Execute(context);

                    if (!tempResult)
                        resultList.Add(new TestResultReason(item.FailureReason));
                }
                catch (Exception e)
                {
                    resultList.Add(new TestResultReason(e.Message));
                }
            }

            if (resultList.Count == 0)
                return new TestResult(true);

            return new TestResult(false, resultList.ToArray());
        }
    }
}