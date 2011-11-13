using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Diagnostics;

namespace DigitMath {
	[DebuggerDisplay("{ToString()}")]
	public class Integer : IComparable<Integer>, IEquatable<Integer> {

		#region Equality
		public bool Equals(Integer a) {
			if (object.Equals(this, null) || object.Equals(a, null)) return false;
			if (a.Sign != this.Sign) return false;
			if (a.digits.Count() != this.digits.Count())
				return false;
			for(int i=0; i < a.digits.Count(); i++){
				if(a.digits[i] != this.digits[i])
					return false;
			}
			return true;
		}

		public override bool Equals(object a) {
			if (object.Equals(this, null) || object.Equals(a, null)) return false;
			Integer b = a as Integer;
			if (b.digits.Count() != this.digits.Count())
				return false;
			for (int i = 0; i < b.digits.Count(); i++) {
				if (b.digits[i] != this.digits[i])
					return false;
			}
			return true;
		}

		public override int GetHashCode() {
			if (digits != null)
				return 17;
			return digits.GetHashCode() * 23 + 97;
		}

		public static bool operator !=(Integer a, Integer b) { return !(a == b); }
		public static bool operator ==(Integer a, Integer b) { return a.Equals(b); }
		#endregion

		#region Comparisons

		public int CompareTo(Integer other) {
			if (other == null) return 1;
			if (other == this) return 0;
			if (this.Sign == false && other.Sign == true) return -1;
			if (this.Sign == true && other.Sign == false) return 1;
			if (this.digits.Count() > other.digits.Count()) {
				if (this.Sign)
					return 1;
				else return -1;
			}
			if (this.digits.Count() < other.digits.Count()) {
				if (this.Sign)
					return 1;
				else return -1;
			}
			for (int i = this.digits.Count(); i >= 0; i--) {
				if (this.digits[i] > other.digits[i]) {
					return 1;
				}
				if (this.digits[i] < other.digits[i]) {
					return -1;
				}
			}
			return 0;
		}

		public static bool operator <(Integer a, Integer b) {
			if (ReferenceEquals(a, b)) return false;
			if (ReferenceEquals(a, null)) return true;
			if (a.digits.Count() < b.digits.Count())
				return true;
			if (b.digits.Count() > a.digits.Count())
				return false;
			for (int i = a.digits.Count() - 1; i >= 0; i--) {
				if (a.digits[i] < b.digits[i])
					return true;
				if (a.digits[i] > b.digits[i])
					return false;
			}
			return false;
		}

		public static bool operator >(Integer a, Integer b) {
			if (ReferenceEquals(a, b)) return false;
			if (ReferenceEquals(a, null)) return true;
			if (a.digits.Count() > b.digits.Count())
				return true;
			if (b.digits.Count() < a.digits.Count())
				return false;
			for (int i = a.digits.Count() - 1; i >= 0; i--) {
				if (a.digits[i] > b.digits[i])
					return true;
				if (a.digits[i] < b.digits[i])
					return false;
			}
			return false;
		}
		public static bool operator <=(Integer a, Integer b) {
			if (ReferenceEquals(a, b)) return true;
			if (ReferenceEquals(a, null)) return true;
			return a.CompareTo(b) <= 0;
		}

		public static bool operator >=(Integer a, Integer b) { return b <= a; }
		#endregion

		/// <summary>0 index is the first digit to the right (ones place)</summary>
		List<Digit> digits = new List<Digit>();
		List<Digit> Digits {
			get { return digits; }
			set {
				this.digits = value;
				for (int i = this.digits.Count() - 1; i >= 0; i--) {
					if (this.digits[i] == Digit.Zero)
						this.digits.RemoveAt(i);
					else break;
				}
			}
		}

		public bool Sign { get; set; }
		public bool IsZero { get; set; }

		public Integer(List<Digit> digits) {
			this.Digits = digits;
		}
		public Integer(List<Digit> digits, bool sign) {
			this.Digits = digits;
			this.Sign = sign;
		}
		public Integer(string inputString) {
			//TODO: implement removal of trailing zeros and parsing of negative sign
			foreach (char c in inputString) {
				this.digits.Insert(0, Digit.CharToDig(c));
			}
		}

		public Integer(string inputString, bool sign) {
			foreach (char c in inputString) {
				this.digits.Insert(0, Digit.CharToDig(c));
			}
			this.Sign = sign;
		}

		public Integer Multiply(Integer b) {
			List<Digit> product = new List<Digit>();
			for (int i = 0; i < b.digits.Count(); i++) {
				Digit[] carriedVals = new Digit[b.digits.Count() - 1];
				for (int j = 0; j < this.digits.Count(); j++) {
					var t = b.digits[0] * this.digits[j];
					//product.Add(t.Last() + carriedVals[j]);
				}
			}
			throw new NotImplementedException();
		}

		public Integer Divide() {
			throw new NotImplementedException();
		}

		public Integer Negate() {
			this.Sign = !this.Sign;
			return this;
		}

		public Integer Absolute() {
			this.Sign = true;
			return this;
		}

		public Digit digitAt(int place) {
			if (place > digits.Count() - 1)
				return Digit.Zero;
			else return digits[place];
		}

		public override string ToString() {
			string stringRep = string.Empty;
			for (int i = digits.Count() - 1; i >= 0; i--) {
				stringRep += digits[i].ToChar();
			}
			return stringRep;
		}

		public static Integer operator +(Integer a, Integer b) {
			return a.Add(b);
		}

		public static Integer operator -(Integer a, Integer b) {
			return a.Subtract(b);
		}

		public Integer Add(Integer b) {
			//TODO: Check for signs, this may actually reduce to a subtraction problem, we're not sure yet
			int digitsInAns = Math.Max(digits.Count(), b.digits.Count()) + 1;
			List<Digit> sumDigits = new List<Digit>(digitsInAns);
			Digit[] carriedDigits = new Digit[digitsInAns];
			for (int i = 0; i < digitsInAns; i++) {
				carriedDigits[i] = Digit.Zero;
			}
			for (int i = 0; i < digitsInAns; i++) {
				//Sum the carried digit and the first digit to add
				var subSum1 = this.digitAt(i) + carriedDigits[i];
				if (subSum1[0] != Digit.Zero) {
					carriedDigits[i + 1] = Digit.One;
				}
				var subSum2 = subSum1[1] + b.digitAt(i);
				if (subSum2[0] != Digit.Zero) {
					carriedDigits[i + 1] = Digit.One;
				}
				sumDigits.Add(subSum2[1]);
			}
			return new Integer(sumDigits);
		}

		public Integer Subtract(Integer b) {
			int digitsInAns = Math.Max(digits.Count(), b.digits.Count());
			if (b > this) {
				return (b - this).Negate();
			}
			Digit[] adjustedFirstDig = new Digit[digitsInAns];
			for (int i = 0; i < digitsInAns; i++) {
				adjustedFirstDig[i] = this.digitAt(i);
			}

			List<Digit> diffDigits = new List<Digit>(digitsInAns);
			for (int i = 0; i < digitsInAns; i++) {
				var diff = adjustedFirstDig[i] - b.digitAt(i);
				if (diff.Item2 == false) {
					diffDigits.Add(diff.Item1.TenMinus());
					int idxToBorrowFrom = int.MinValue;
					for (int j = i + 1; j < digitsInAns; j++) {
						if (j < digitsInAns && this.digits[j] > Digit.Zero) {
							idxToBorrowFrom = j;
							j = digitsInAns;
						}
					}
					if (idxToBorrowFrom != int.MinValue) {
						for (int j = i; j < idxToBorrowFrom; j++) {
							adjustedFirstDig[i] = Digit.Nine;
						}
						adjustedFirstDig[idxToBorrowFrom] = (this.digits[idxToBorrowFrom] - Digit.One).Item1;
					}
				} else {
					diffDigits.Add(diff.Item1);
				}
			}
			return new Integer(diffDigits);
		}
	}
}
