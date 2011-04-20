﻿using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;

using Dicom;
using Dicom.Data;
using Dicom.Utility;

namespace Dicom.Data {
	public enum DicomMatchOperator : byte {
		/// <summary>All rules match</summary>
		And,

		/// <summary>Any rule matches</summary>
		Or
	}

	public interface IDicomMatchRule {
		bool Match(DcmDataset dataset);
	}

	public class DicomMatch : IDicomMatchRule {
		#region Private Members
		private DicomMatchOperator _operator;
		private IList<IDicomMatchRule> _rules;
		#endregion

		#region Public Constructor
		public DicomMatch() {
			_rules = new List<IDicomMatchRule>();
			_operator = DicomMatchOperator.And;
		}

		public DicomMatch(DicomMatchOperator op) {
			_rules = new List<IDicomMatchRule>();
			_operator = op;
		}

		public DicomMatch(params IDicomMatchRule[] rules) {
			_rules = new List<IDicomMatchRule>(rules);
			_operator = DicomMatchOperator.And;
		}

		public DicomMatch(DicomMatchOperator op, params IDicomMatchRule[] rules) {
			_rules = new List<IDicomMatchRule>(rules);
			_operator = op;
		}
		#endregion

		#region Public Properties
		public DicomMatchOperator Operator {
			get { return _operator; }
			set { _operator = value; }
		}
		#endregion

		#region Public Methods
		public void Add(IDicomMatchRule rule) {
			_rules.Add(rule);
		}

		public bool Match(DcmDataset dataset) {
			if (_operator == DicomMatchOperator.Or) {
				foreach (IDicomMatchRule rule in _rules)
					if (rule.Match(dataset))
						return true;

				return false;
			}
			else {
				foreach (IDicomMatchRule rule in _rules)
					if (!rule.Match(dataset))
						return false;

				return true;
			}
		}

		public override string ToString() {
			StringBuilder sb = new StringBuilder();
			foreach (IDicomMatchRule rule in _rules) {
				if (rule is DicomMatch)
					sb.AppendLine("(((").AppendLine(rule.ToString()).AppendLine(")))");
				else
					sb.AppendLine(rule.ToString());
			}
			return sb.ToString();
		}
		#endregion
	}

	/// <summary>
	/// Negates the return value of a match rule.
	/// </summary>
	public class NegativeDicomMatchRule : IDicomMatchRule {
		#region Private Members
		private IDicomMatchRule _rule;
		#endregion

		#region Public Constructor
		public NegativeDicomMatchRule(IDicomMatchRule rule) {
			_rule = rule;
		}
		#endregion

		#region Public Methods
		public bool Match(DcmDataset dataset) {
			return !_rule.Match(dataset);
		}

		public override string ToString() {
			return String.Format("not {0}", _rule);
		}
		#endregion
	}

	/// <summary>
	/// Checks that a DICOM element exists.
	/// </summary>
	public class ExistsDicomMatchRule : IDicomMatchRule {
		#region Private Members
		private DicomTag _tag;
		#endregion

		#region Public Constructor
		public ExistsDicomMatchRule(DicomTag tag) {
			_tag = tag;
		}
		#endregion

		#region Public Methods
		public bool Match(DcmDataset dataset) {
			return dataset.Contains(_tag);
		}

		public override string ToString() {
			return String.Format("{0} exists", _tag);
		}
		#endregion
	}

	/// <summary>
	/// Checks if a DICOM element exists and has a value.
	/// </summary>
	public class EmptyDicomMatchRule : IDicomMatchRule {
		#region Private Members
		private DicomTag _tag;
		#endregion

		#region Public Constructor
		public EmptyDicomMatchRule(DicomTag tag) {
			_tag = tag;
		}
		#endregion

		#region Public Methods
		public bool Match(DcmDataset dataset) {
			if (dataset.Contains(_tag)) {
				string value = dataset.GetValueString(_tag).Trim();
				return String.IsNullOrEmpty(value);
			}
			return true;
		}

		public override string ToString() {
			return String.Format("{0} empty", _tag);
		}
		#endregion
	}

	/// <summary>
	/// Compares a DICOM element value against a string.
	/// </summary>
	public class EqualsDicomMatchRule : IDicomMatchRule {
		#region Private Members
		private DicomTag _tag;
		private string _value;
		#endregion

		#region Public Constructor
		public EqualsDicomMatchRule(DicomTag tag, string value) {
			_tag = tag;
			_value = value;
		}
		#endregion

		#region Public Methods
		public bool Match(DcmDataset dataset) {
			if (dataset.Contains(_tag)) {
				string value = dataset.GetValueString(_tag);
				return _value == value;
			}
			return false;
		}

		public override string ToString() {
			return String.Format("{0} equals '{1}'", _tag, _value);
		}
		#endregion
	}

	/// <summary>
	/// Checks if a DICOM element value starts with a string.
	/// </summary>
	public class StartsWithDicomMatchRule : IDicomMatchRule {
		#region Private Members
		private DicomTag _tag;
		private string _value;
		#endregion

		#region Public Constructor
		public StartsWithDicomMatchRule(DicomTag tag, string value) {
			_tag = tag;
			_value = value;
		}
		#endregion

		#region Public Methods
		public bool Match(DcmDataset dataset) {
			if (dataset.Contains(_tag)) {
				string value = dataset.GetValueString(_tag);
				return value.StartsWith(_value);
			}
			return false;
		}

		public override string ToString() {
			return String.Format("{0} starts with '{1}'", _tag, _value);
		}
		#endregion
	}

	/// <summary>
	/// Checks if a DICOM element value ends with a string.
	/// </summary>
	public class EndsWithDicomMatchRule : IDicomMatchRule {
		#region Private Members
		private DicomTag _tag;
		private string _value;
		#endregion

		#region Public Constructor
		public EndsWithDicomMatchRule(DicomTag tag, string value) {
			_tag = tag;
			_value = value;
		}
		#endregion

		#region Public Methods
		public bool Match(DcmDataset dataset) {
			if (dataset.Contains(_tag)) {
				string value = dataset.GetValueString(_tag);
				return value.EndsWith(_value);
			}
			return false;
		}

		public override string ToString() {
			return String.Format("{0} ends with '{1}'", _tag, _value);
		}
		#endregion
	}

	/// <summary>
	/// Checks if a DICOM element value contains a string.
	/// </summary>
	public class ContainsDicomMatchRule : IDicomMatchRule {
		#region Private Members
		private DicomTag _tag;
		private string _value;
		#endregion

		#region Public Constructor
		public ContainsDicomMatchRule(DicomTag tag, string value) {
			_tag = tag;
			_value = value;
		}
		#endregion

		#region Public Methods
		public bool Match(DcmDataset dataset) {
			if (dataset.Contains(_tag)) {
				string value = dataset.GetValueString(_tag);
				return value.Contains(_value);
			}
			return false;
		}

		public override string ToString() {
			return String.Format("{0} contains '{1}'", _tag, _value);
		}
		#endregion
	}

	/// <summary>
	/// Matches a wildcard pattern against a DICOM element value.
	/// </summary>
	public class WildcardDicomMatchRule : IDicomMatchRule {
		#region Private Members
		private DicomTag _tag;
		private string _pattern;
		#endregion

		#region Public Constructor
		public WildcardDicomMatchRule(DicomTag tag, string pattern) {
			_tag = tag;
			_pattern = pattern;
		}
		#endregion

		#region Public Methods
		public bool Match(DcmDataset dataset) {
			if (dataset.Contains(_tag)) {
				string value = dataset.GetValueString(_tag);
				return Wildcard.Match(_pattern, value);
			}
			return false;
		}

		public override string ToString() {
			return String.Format("{0} wildcard match '{1}'", _tag, _pattern);
		}
		#endregion
	}

	/// <summary>
	/// Matches regular expression pattern against a DICOM element value.
	/// </summary>
	public class RegexDicomMatchRule : IDicomMatchRule {
		#region Private Members
		private DicomTag _tag;
		private string _pattern;
		private Regex _regex;
		#endregion

		#region Public Constructor
		public RegexDicomMatchRule(DicomTag tag, string pattern) {
			_tag = tag;
			_pattern = pattern;
			_regex = new Regex(_pattern);
		}
		#endregion

		#region Public Methods
		public bool Match(DcmDataset dataset) {
			if (dataset.Contains(_tag)) {
				string value = dataset.GetValueString(_tag);
				return _regex.IsMatch(value);
			}
			return false;
		}

		public override string ToString() {
			return String.Format("{0} regex match '{1}'", _tag, _pattern);
		}
		#endregion
	}

	/// <summary>
	/// Matches a DICOM element value against a set of strings.
	/// </summary>
	public class OneOfDicomMatchRule : IDicomMatchRule {
		#region Private Members
		private DicomTag _tag;
		private string[] _values;
		#endregion

		#region Public Constructor
		public OneOfDicomMatchRule(DicomTag tag, params string[] values) {
			_tag = tag;
			_values = values;
		}
		#endregion

		#region Public Methods
		public bool Match(DcmDataset dataset) {
			if (dataset.Contains(_tag)) {
				string value = dataset.GetValueString(_tag);
				foreach (string v in _values)
					if (v == value)
						return true;
			}
			return false;
		}

		public override string ToString() {
			StringBuilder sb = new StringBuilder();
			sb.AppendFormat("{0} is one of ['", _tag);
			for (int i = 0; i < _values.Length; i++) {
				if (i > 0)
					sb.Append("', '");
				sb.Append(_values[i]);
			}
			sb.Append("']");
			return sb.ToString();
		}
		#endregion
	}
}
