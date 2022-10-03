using System;
using System.Windows;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Text;
using NewIVEdit;

public static class InputStringGenerator
{
    static InputStringGenerator()
    {
        int idx = 0;
        foreach (var c in BASE32Table)
        {
            BASE32Dic.Add(c, idx++);
        }
    }
    private static SortedList<char, int> BASE32Dic = new SortedList<char, int>();
    private static char[] BASE32Table = new char[32]
    {
        '0','1','2','3','4','5','6','7','8','9',
        'A','B','C','D','E','F','G','H','I','J',
        'K','L','M','N','O','P','Q','R','S','T',
        'U','V'
    };
 
    public static string RiceCodeModoki32(string input, int K)
    {
        //input % 2^K と input / 2^K を それぞれ log32(2^K)文字=ceiling(K/5)文字でコード化する
        ulong factor = 1;
        factor <<= K;
        ulong temp = 0;
        ulong d = 0;
        ulong r = 0;
        int Length = (K + 4) / 5;
        var deletecomma = new Regex(@"\,");
        input = deletecomma.Replace(input, String.Empty);
        if (!ulong.TryParse(input, out temp))
        {
            return new string('0', Length * 2);
        }
        d = temp / factor;
        string res = EncBASE32(d, Length);
        r = temp % factor;
        res += EncBASE32(r, Length);
        return res;
    }
    public static ulong RevRiceCodeModoki32(string input, int K)
    {
        ulong factor = 1;
        factor <<= K;
        int Length = (K + 4) / 5;
        if (input.Length < 2 * K) return 0UL;
        string baseStr = input.Substring(0, Length);
        string resiStr = input.Substring(Length, Length);
        ulong res = RevBase32(baseStr) * factor + RevBase32(resiStr);
        return res;
    }


    public static string EncBASE32(ulong input, int Length)
    {
        int[] ls = new int[Length];
        ulong d = input / 32;
        ulong r = input % 32;
        int idx = Length - 1;
        while (d != 0)
        {
            ls[idx] = (int)r;
            r = d % 32;
            d /= 32;
            idx--;
        }
        ls[idx] = (int)r;
        string res = "";
        foreach (int i in ls)
        {
            res += BASE32Table[i].ToString();
        }
        return res;
    }

    public static ulong RevBase32(string input)
    {
        ulong res = 0UL;
        int length = input.Length;
        ulong factor = 1;
        for (int i = length - 1; i > -1; i--)
        {
            char c = input[i];
            if (BASE32Dic.ContainsKey(c))
            {
                res += factor * (ulong)BASE32Dic[c];
            }
            res <<= 5;
        }
        return res;
    }

    public static string EncBASE32(List<int> input)
    {
        StringBuilder sb = new StringBuilder();
        foreach (var c in input)
        {
            if (c < 0 || c > 31) continue;
            sb.Append(BASE32Table[c]);
        }
        return sb.ToString();
    }

    public static List<int> DecBASE32(string input)
    {
        List<int> res = new List<int>();
        foreach (var c in input)
        {
            if (BASE32Dic.ContainsKey(c)) res.Add(BASE32Dic[c]);
        }
        return res;
    }

    public static string EncBASE44(ulong input, int Length)
    {
        char[] BASE44Table = new char[44]
        {
            '0','1','2','3','4','5','6','7','8','9',
            'A','B','C','D','E','F','G','H','I','J',
            'K','L','M','N','O','P','Q','R','S','T',
            'U','V','W','X','Y','Z',' ','-','.','$',
            '*','+','/',':'
        };
        int[] ls = new int[Length];
        ulong d = input / 44;
        ulong r = input % 44;
        int idx = Length - 1;
        while (d != 0)
        {
            ls[idx] = (int)r;
            r = d % 44;
            d /= 44;
            idx--;
        }
        ls[idx] = (int)r;
        string res = "";
        foreach (int i in ls)
        {
            res += BASE44Table[i].ToString();
        }
        return res;
    }

    public static class SufArrayDic
    {
        static SufArrayDic()
        {
            Dic = new SortedList<char, short>()
            {
                {'\0',0 },{ '0',1 },{'1', 2},{'2' ,3},{'3', 4},{'4',5 },{'5',6 },{'6',7 },{'7',8 },{'8',9 },{'9',10 },
                { 'A',11 },{'B',12 },{'C',13 },{'D',14 },{'E',15 },{'F',16 },{'G',17 },{'H',18 },{'I',19 },{'J',20 },
                { 'K',21 },{'L',22 },{'M',23 },{'N',24 },{'O',25 },{'P',26 },{'Q',27 },{'R',28 },{'S',29 },{'T',30 },
                { 'U',31 },{'V',32 },{ 'W',33},{ 'X',34},{'Y',35},{ 'Z',36},{' ',37},{'-',38},{'.',39},{',',40},
                {'$',41},{ '*',42},{'+',43},{'%',44},{'/',45},{':',46},{'&',47}
            };
            InvDic = new SortedList<short, char>();
            foreach (var d in Dic)
            {
                InvDic.Add(d.Value, d.Key);
            }
        }
        //Start From Zero
        public static SortedList<char, short> Dic;
        public static SortedList<short, char> InvDic;

    }
    public class SuffixArray
    {
        public class SuffixCharacter : IComparable<SuffixCharacter>
        {
            public int Prio;
            public string RawStr;
            public SuffixCharacter(string input)
            {
                RawStr = input;
            }
            public int Compare(SuffixCharacter x, SuffixCharacter y)
            {
                if (x.Prio < y.Prio) return -1;
                else if (x.Prio > y.Prio) return 1;
                else return 0;

            }

            public int CompareRawString(SuffixCharacter x, SuffixCharacter y)
            {
                int minLen = Math.Min(x.RawStr.Length, y.RawStr.Length);
                for (int i = 0; i < minLen; i++)
                {
                    if (SufArrayDic.Dic[x.RawStr[i]] < SufArrayDic.Dic[y.RawStr[i]]) return -1;
                    else if (SufArrayDic.Dic[x.RawStr[i]] > SufArrayDic.Dic[y.RawStr[i]]) return 1;
                }
                if (x.RawStr.Length < y.RawStr.Length) return -1;
                else if (x.RawStr.Length > y.RawStr.Length) return 1;
                else return 0;
            }
            public int CompareToRawstring(SuffixCharacter y)
            {
                return CompareRawString(this, y);
            }
            public int CompareTo(SuffixCharacter y)
            {
                return Compare(this, y);
            }
        }
        public class SuffixArrayString : IComparable<SuffixArrayString>
        {
            public int Length;
            public SuffixCharacter[] Str;

            public SuffixArrayString(string input)
            {
                Length = input.Length;
                Str = new SuffixCharacter[input.Length];
                int idx = 0;
                foreach (char c in input)
                {
                    Str[idx] = new SuffixCharacter(c.ToString());
                    if (SufArrayDic.Dic.ContainsKey(c))
                    {
                        Str[idx].Prio = SufArrayDic.Dic[c];
                    }
                    else
                    {
                        Str[idx].Prio = 0;
                    }
                    idx++;
                }
            }

            public SuffixArrayString(SuffixCharacter[] LMSSubstring, int[] Rank, int from, int to)
            {
                Length = to - from + 1;
                Str = new SuffixCharacter[to - from + 1];
                for (var i = 0; i < to - from + 1; i++)
                {
                    Str[i] = LMSSubstring[from + i];
                    Str[i].Prio = Rank[from + i];
                }
            }

            public int Compare(SuffixArrayString x, SuffixArrayString y)
            {
                var minLen = Math.Min(x.Length, y.Length);
                for (int i = 0; i < minLen; i++)
                {
                    if (x.Str[i].CompareTo(y.Str[i]) != 0) return x.Str[i].CompareTo(y.Str[i]);
                }
                if (x.Length < y.Length) return -1;
                else if (x.Length > y.Length) return 1;
                else return 0;
            }
            public int CompareTo(SuffixArrayString y)
            {
                return Compare(this, y);
            }


        }
        private short[] _suffixType = new short[1];
        private int[] _bucket;
        private int[] _bucket_prev;
        private SuffixArrayString[] _data;
        public int Length;
        public int AlphabetSize;
        public SuffixArray(string input)
        {
            AlphabetSize = SufArrayDic.Dic.Count;
            Array.Resize<int>(ref _bucket, AlphabetSize);
            Array.Resize<int>(ref _bucket_prev, AlphabetSize);
            if (input.Length > 1)
            {
                Array.Resize<short>(ref _suffixType, input.Length);
                Length = input.Length;
            }
            else
            {
                return;
            }
            int idx = 0;
            _data = new SuffixArrayString[Length];
            foreach (char c in input)
            {
                _data[idx] = new SuffixArrayString(input.Substring(idx));
                int _buk_key = SufArrayDic.Dic[c];
                _bucket[_buk_key]++;
                idx++;
            }
            if (_bucket.Length > 0)
            {
                _bucket_prev[0] = 0;
                var prev = _bucket[0];
                for (int i = 1; i < _bucket.Length; i++)
                {
                    _bucket[i] += prev;
                    _bucket_prev[i] = prev;
                    prev = _bucket[i];
                }

            }
            if (Length > 1)
            {
                _suffixType[Length - 1] = -1;
                for (int i = Length - 1; i > 0; i--)
                {
                    if (_data[i - 1].CompareTo(_data[i]) < 1)
                    {
                        _suffixType[i - 1] = -1;
                    }
                    else
                    {
                        _suffixType[i - 1] = 1;
                    }
                    if (_suffixType[i] == -1 && _suffixType[i - 1] == 1) _suffixType[i] = 0;
                }
            }
        }

        protected SuffixArray(SuffixCharacter[] LMSSubstring, int[] Rank)
        {
            AlphabetSize = Rank.Max() + 1;
            if (LMSSubstring.Length != Rank.Length) return;
            if (LMSSubstring.Length < 1) return;
            Length = LMSSubstring.Length;
            Array.Resize<SuffixArrayString>(ref _data, Length);
            Array.Resize<short>(ref _suffixType, Length);
            Array.Resize<int>(ref _bucket, AlphabetSize);
            Array.Resize<int>(ref _bucket_prev, AlphabetSize);
            for (int i = 0; i < Length; i++)
            {
                _data[i] = new SuffixArrayString(LMSSubstring, Rank, i, Length - 1);
                _bucket[Rank[i]]++;
            }
            if (_bucket.Length > 0)
            {
                var prev = _bucket[0];
                for (int i = 1; i < _bucket.Length; i++)
                {
                    _bucket[i] += prev;
                    _bucket_prev[i] = prev;
                    prev = _bucket[i];
                }

            }
            if (Length > 1)
            {
                _suffixType[Length - 1] = -1;
                for (int i = Length - 1; i > 0; i--)
                {
                    if (_data[i - 1].CompareTo(_data[i]) < 1)
                    {
                        _suffixType[i - 1] = -1;
                    }
                    else
                    {
                        _suffixType[i - 1] = 1;
                    }
                    if (_suffixType[i] == -1 && _suffixType[i - 1] == 1) _suffixType[i] = 0;
                }
            }
            else if (Length == 1)
            {
                _suffixType[0] = -1;
            }
        }

        protected void SAIS_init(ref int[] resRank, ref SuffixCharacter[] resSubString)
        {
            //LMSを先頭文字のみについてソートする（バケツソート)
            SuffixArrayString[] lmsSuffix = new SuffixArrayString[Length];
            int[] oriPos = new int[Length];
            int[] temp_bucket = new int[_bucket.Length];
            Array.Copy(_bucket, temp_bucket, _bucket.Length);

            for (int i = 0; i < _data.Length; i++)
            {
                if (_suffixType[i] != 0) continue;
                var d = _data[i];
                var c = d.Str[0];
                int buk_key = c.Prio;
                lmsSuffix[--temp_bucket[buk_key]] = d;
                oriPos[temp_bucket[buk_key]] = i;
            }
            Array.Copy(_bucket_prev, temp_bucket, _bucket.Length);
            SuffixArrayString[] exLmsSuffix = new SuffixArrayString[Length];
            int[] exOriPos = Enumerable.Repeat<int>(-1, Length).ToArray();
            for (int i = 0; i < lmsSuffix.Length; i++)
            {
                var suf = lmsSuffix[i];
                int o = oriPos[i];
                int e = exOriPos[i];
                if (lmsSuffix[i] != null)
                {
                    if (_suffixType[o - 1] == 1) //LMSのひとつ前がL
                    {
                        var d = _data[o - 1];
                        int buk_key = d.Str[0].Prio;
                        exOriPos[temp_bucket[buk_key]] = o - 1;
                        exLmsSuffix[temp_bucket[buk_key]++] = d;
                    }
                }
                else if (exLmsSuffix[i] != null && _suffixType[e] == 1 && e != 0)
                {
                    if (_suffixType[e - 1] == 1) //LMSのひとつ前がL
                    {
                        var d = _data[e - 1];
                        int buk_key = d.Str[0].Prio;
                        exOriPos[temp_bucket[buk_key]] = e - 1;
                        exLmsSuffix[temp_bucket[buk_key]++] = d;
                    }
                }
            }
            Array.Copy(_bucket, temp_bucket, _bucket.Length);
            for (int i = exLmsSuffix.Length - 1; i > -1; i--)
            {
                if (exLmsSuffix[i] == null) continue;
                var suf = exLmsSuffix[i];
                var c = suf.Str[0];
                int o = exOriPos[i];
                if (o == 0) continue;
                if (_suffixType[o - 1] == -1 || _suffixType[o - 1] == 0)
                {
                    var d = _data[o - 1];
                    int buk_key = d.Str[0].Prio;
                    exLmsSuffix[--temp_bucket[buk_key]] = d;
                    exOriPos[temp_bucket[buk_key]] = o - 1;
                }
            }
            exLmsSuffix[0] = _data[Length - 1];
            exOriPos[0] = Length - 1;


            int[] InvOriPos = new int[Length];
            int idx = 0;
            foreach (var val in exOriPos)
            {
                if (val == -1) continue;
                InvOriPos[val] = idx++;
            }
            int[] rankExist = new int[Length];
            for (int i = 0; i < Length; i++)
            {
                if (_suffixType[i] == 0) rankExist[InvOriPos[i]]++;
            }
            int maxRank = -1;
            for (int i = 0; i < Length; i++)
            {
                if (exOriPos[i] == -1) continue;
                if (rankExist[i] > 0)
                {
                    int j = i;
                    while (j > 0 && rankExist[j - 1] == 0) j--;
                    InvOriPos[exOriPos[i]] = j;
                    if (j > maxRank) maxRank = j;
                    rankExist[i] = 0;
                    rankExist[j] = 1;
                }
            }
            SuffixCharacter[] lmsSubString = new SuffixCharacter[maxRank + 1];
            int[] lmsSubRank = new int[maxRank + 1];
            int substridx = 0;
            for (int i = 1; i < Length; i++)
            {
                if (_suffixType[i] == 0)
                {
                    int substr_rank = InvOriPos[i];
                    int offset = 1;
                    string lmsrawstring = _data[i].Str[0].RawStr;
                    while (offset < Length - i && _suffixType[i + offset] != 0)
                    {
                        lmsrawstring += _data[i].Str[offset].RawStr;
                        offset++;
                    }
                    if (offset < Length - i)
                    {
                        lmsrawstring += _data[i].Str[offset].RawStr;
                    }
                    SuffixCharacter c = new SuffixCharacter(lmsrawstring);
                    lmsSubRank[substridx] = substr_rank;
                    c.Prio = substr_rank;
                    lmsSubString[substridx++] = c;
                }
            }
            int[] invLmsSubRank = new int[maxRank + 1];
            for (int i = 0; i < maxRank + 1; i++)
            {
                invLmsSubRank[lmsSubRank[i]] = i;
            }
            int newrank = 0;
            for (int oldrank = 0; oldrank < maxRank + 1; oldrank++)
            {
                lmsSubRank[invLmsSubRank[oldrank]] = newrank;
                while (oldrank < maxRank && lmsSubString[invLmsSubRank[oldrank]].CompareToRawstring(lmsSubString[invLmsSubRank[oldrank + 1]]) == 0)
                {
                    lmsSubRank[invLmsSubRank[oldrank + 1]] = newrank;
                    oldrank++;
                }
                newrank++;
            }

            resRank = lmsSubRank;
            resSubString = lmsSubString;

        }
        protected Tuple<SuffixArrayString[], int[]> SAIS_iter(/*ref int[] lmsSubRank, ref SuffixCharacter[] lmsSubString*/)
        {
            int[] lmsSubRank = null;
            SuffixCharacter[] lmsSubString = null;
            SAIS_init(ref lmsSubRank, ref lmsSubString);
            var newsuf = new SuffixArray(lmsSubString, lmsSubRank);
            int[] newLmsSubRank = null;
            SuffixCharacter[] newLmsSubstring = null;
            newsuf.SAIS_init(ref newLmsSubRank, ref newLmsSubstring);
            int[] bucket = new int[newLmsSubRank.Length];
            bool isOK = true;
            foreach (int a in newLmsSubRank) bucket[a]++;
            foreach (int b in bucket) if (b > 1)
                {
                    isOK = false;
                    break;
                }
            //コーナーケース
            //lmsSubRank.Count = 1
            //→TypeS*が末尾文字のみ
            if (lmsSubRank.Count() == 1)
            {
                newLmsSubstring = lmsSubString;
                newLmsSubRank = lmsSubRank;
            }
            if (isOK)
            {
                var newSA = newsuf.SAIS_final(ref newLmsSubstring, ref newLmsSubRank);

                int lmsAlpahbetsize = lmsSubRank.Length;
                int[] invLmsSubrank = new int[lmsAlpahbetsize];
                for (int i = 0; i < lmsAlpahbetsize; i++)
                {
                    invLmsSubrank[i] = newSA.Item2[i];
                }
                int[] lmsSubstridx = new int[lmsAlpahbetsize];
                int idx = 0;
                for (int i = 0; i < Length; i++)
                {
                    if (_suffixType[i] == 0)
                    {
                        lmsSubstridx[idx] = i;
                        idx++;
                    }
                    if (idx > lmsAlpahbetsize) break;
                }
                int[] invLmsSubstridx = new int[lmsAlpahbetsize];
                for (int i = 0; i < lmsAlpahbetsize; i++)
                {
                    invLmsSubstridx[i] = lmsSubstridx[invLmsSubrank[i]];
                }
                SuffixArrayString[] tempSA = new SuffixArrayString[Length];
                int[] tempSAindex = Enumerable.Repeat(-1, Length).ToArray();
                int[] temp_bucket = new int[_bucket.Length];
                Array.Copy(_bucket, temp_bucket, _bucket.Length);
                for (int i = lmsAlpahbetsize - 1; i > -1; i--)
                {
                    var d = _data[invLmsSubstridx[i]];
                    tempSAindex[--temp_bucket[d.Str[0].Prio]] = invLmsSubstridx[i];
                    tempSA[temp_bucket[d.Str[0].Prio]] = d;
                }
                int[] SAindex = Enumerable.Repeat(-1, Length).ToArray();
                SuffixArrayString[] SA = new SuffixArrayString[Length];
                for (int i = 0; i < Length; i++)
                {
                    int o = tempSAindex[i];
                    int e = SAindex[i];
                    if (o != 0 && o != -1)
                    {
                        if (_suffixType[o] == 0 && _suffixType[o - 1] == 1)
                        {
                            var d = _data[o - 1];
                            SAindex[_bucket_prev[d.Str[0].Prio]] = o - 1;
                            SA[_bucket_prev[d.Str[0].Prio]++] = d;
                        }
                    }
                    if (e != 0 && e != -1)
                    {
                        if (_suffixType[e] == 1 && _suffixType[e - 1] == 1)
                        {
                            var d = _data[e - 1];
                            SAindex[_bucket_prev[d.Str[0].Prio]] = e - 1;
                            SA[_bucket_prev[d.Str[0].Prio]++] = d;
                        }
                    }
                }
                for (int i = Length - 1; i > -1; i--)
                {
                    int saidx = SAindex[i];
                    if (saidx == 0 || saidx == -1) continue;
                    if (_suffixType[saidx - 1] == -1 || _suffixType[saidx - 1] == 0)
                    {
                        var d = _data[saidx - 1];
                        SA[--_bucket[d.Str[0].Prio]] = d;
                        SAindex[_bucket[d.Str[0].Prio]] = saidx - 1;
                    }
                }
                SA[0] = _data[Length - 1];
                SAindex[0] = Length - 1;
                return new Tuple<SuffixArrayString[], int[]>(SA, SAindex);
            }
            else
            {
                var newSA = newsuf.SAIS_iter();

                int lmsAlpahbetsize = lmsSubRank.Length;
                int[] invLmsSubrank = new int[lmsAlpahbetsize];
                for (int i = 0; i < lmsAlpahbetsize; i++)
                {
                    invLmsSubrank[i] = newSA.Item2[i];
                }
                int[] lmsSubstridx = new int[lmsAlpahbetsize];
                int idx = 0;
                for (int i = 0; i < Length; i++)
                {
                    if (_suffixType[i] == 0)
                    {
                        lmsSubstridx[idx] = i;
                        idx++;
                    }
                    if (idx > lmsAlpahbetsize) break;
                }
                int[] invLmsSubstridx = new int[lmsAlpahbetsize];
                for (int i = 0; i < lmsAlpahbetsize; i++)
                {
                    invLmsSubstridx[i] = lmsSubstridx[invLmsSubrank[i]];
                }
                SuffixArrayString[] tempSA = new SuffixArrayString[Length];
                int[] tempSAindex = Enumerable.Repeat(-1, Length).ToArray();
                int[] temp_bucket = new int[_bucket.Length];
                Array.Copy(_bucket, temp_bucket, _bucket.Length);
                for (int i = lmsAlpahbetsize - 1; i > -1; i--)
                {
                    var d = _data[invLmsSubstridx[i]];
                    tempSAindex[--temp_bucket[d.Str[0].Prio]] = invLmsSubstridx[i];
                    tempSA[temp_bucket[d.Str[0].Prio]] = d;
                }
                SuffixArrayString[] SA = new SuffixArrayString[Length];
                int[] SAindex = Enumerable.Repeat(-1, Length).ToArray();
                for (int i = 0; i < Length; i++)
                {
                    int o = tempSAindex[i];
                    int e = SAindex[i];
                    if (o != 0 && o != -1)
                    {
                        if (_suffixType[o] == 0 && _suffixType[o - 1] == 1)
                        {
                            var d = _data[o - 1];
                            SAindex[_bucket_prev[d.Str[0].Prio]] = o - 1;
                            SA[_bucket_prev[d.Str[0].Prio]++] = d;
                        }
                    }
                    else if (e != 0 && e != -1)
                    {
                        if (_suffixType[e] == 1 && _suffixType[e - 1] == 1)
                        {
                            var d = _data[e - 1];
                            SAindex[_bucket_prev[d.Str[0].Prio]] = e - 1;
                            SA[_bucket_prev[d.Str[0].Prio]++] = d;
                        }
                    }
                }
                for (int i = Length - 1; i > -1; i--)
                {
                    int saidx = SAindex[i];
                    if (saidx == 0 || saidx == -1) continue;
                    if (_suffixType[saidx - 1] == -1 || _suffixType[saidx - 1] == 0)
                    {
                        var d = _data[saidx - 1];
                        SA[--_bucket[d.Str[0].Prio]] = d;
                        SAindex[_bucket[d.Str[0].Prio]] = saidx - 1;
                    }
                }
                SA[0] = _data[Length - 1];
                SAindex[0] = Length - 1;
                return new Tuple<SuffixArrayString[], int[]>(SA, SAindex);
            }

        }
        protected Tuple<SuffixArrayString[], int[]> SAIS_final(ref SuffixCharacter[] newLmsSubstring, ref int[] newLmsSubRank)
        {

            SuffixArrayString[] SATemp = new SuffixArrayString[this.Length];
            int[] SAindexTemp = Enumerable.Repeat(-1, Length).ToArray();
            int[] temp_bucket = new int[_bucket.Length];
            Array.Copy(_bucket, temp_bucket, _bucket.Length);
            int AlphabetSize = newLmsSubRank.Max() + 1;
            int idx = 0;
            int[] newLmsSubstridx = new int[AlphabetSize];
            for (int i = 0; i < Length; i++)
            {
                if (_suffixType[i] == 0)
                {
                    newLmsSubstridx[idx] = i;
                    idx++;
                }
                if (idx > AlphabetSize) break;
            }

            int[] invNewLmsSubRank = new int[AlphabetSize];
            for (int i = 0; i < AlphabetSize; i++)
            {
                invNewLmsSubRank[newLmsSubRank[i]] = i;
            }
            int[] invNewLmsSubstridx = new int[AlphabetSize];
            for (int i = 0; i < AlphabetSize; i++)
            {
                invNewLmsSubstridx[i] = newLmsSubstridx[invNewLmsSubRank[i]];
            }


            for (int i = AlphabetSize - 1; i > -1; i--)
            {
                var d = _data[invNewLmsSubstridx[i]];
                SATemp[--temp_bucket[d.Str[0].Prio]] = d;
                SAindexTemp[temp_bucket[d.Str[0].Prio]] = invNewLmsSubstridx[i];
            }
            SuffixArrayString[] SA = new SuffixArrayString[this.Length];
            int[] SAindex = Enumerable.Repeat(-1, Length).ToArray();
            for (int i = 0; i < this.Length; i++)
            {
                int o = SAindexTemp[i];
                int e = SAindex[i];
                if (o != 0 && o != -1)
                {
                    if (_suffixType[o] == 0 && _suffixType[o - 1] == 1)
                    {
                        var d = _data[o - 1];
                        SAindex[_bucket_prev[d.Str[0].Prio]] = o - 1;
                        SA[_bucket_prev[d.Str[0].Prio]++] = d;
                    }
                }
                if (e != 0 && e != -1)
                {
                    if (_suffixType[e] == 1 && _suffixType[e - 1] == 1)
                    {
                        var d = _data[e - 1];
                        SAindex[_bucket_prev[d.Str[0].Prio]] = e - 1;
                        SA[_bucket_prev[d.Str[0].Prio]++] = d;
                    }
                }
            }
            for (int i = this.Length - 1; i > -1; i--)
            {
                int saidx = SAindex[i];
                if (saidx == 0 || saidx == -1) continue;
                if (_suffixType[saidx - 1] == -1 || _suffixType[saidx - 1] == 0)
                {
                    var d = _data[saidx - 1];
                    SA[--_bucket[d.Str[0].Prio]] = d;
                    SAindex[_bucket[d.Str[0].Prio]] = saidx - 1;
                }
            }
            SA[0] = _data[this.Length - 1];
            SAindex[0] = this.Length - 1;
            return new Tuple<SuffixArrayString[], int[]>(SA, SAindex);
        }
        public Tuple<SuffixArrayString[], int[]> SAIS()
        {
            return SAIS_iter();
        }
    }

    public static string BWTransform(string input, int[] SAIndex)
    {
        if (input.Length != SAIndex.Length) return "";
        StringBuilder sb = new StringBuilder(new string('A', input.Length));

        for (int i = 0; i < input.Length; i++)
        {
            int saidx = SAIndex[i];
            if (saidx == 0)
            {
                sb[i] = input[input.Length - 1];
            }
            else
            {
                sb[i] = input[saidx - 1];
            }
        }
        return sb.ToString();
    }

    public static string BWInverseTransform(string input, int initPos = -1)
    {
        var tempString = new List<int>();
        foreach (var c in input)
        {
            if (SufArrayDic.Dic.ContainsKey(c))
            {
                tempString.Add(SufArrayDic.Dic[c]);
            }
        }
        var C = new SortedList<int, int>();
        foreach (var t in tempString)
        {
            if (C.ContainsKey(t))
            {
                C[t]++;
            }
            else
            {
                C.Add(t, 1);
            }
        }
        int prev = 0;
        var tempC = new SortedList<int, int>();
        foreach (var c in C)
        {
            tempC.Add(c.Key, prev);
            prev += c.Value;
        }
        C = tempC;
        int[] mu = new int[input.Length];
        int idx = 0;
        foreach (var c in tempString)
        {
            mu[C[c]] = idx;
            C[c]++;
            idx++;
        }
        if (initPos == -1)
        {
            for (initPos = 0; initPos < tempString.Count; initPos++)
            {
                if (tempString[initPos] == 0) break;
            }
        }
        int[] tempRes = new int[input.Length];
        int pos = initPos;
        for (int i = 0; i < tempString.Count; i++)
        {
            pos = mu[pos];
            tempRes[i] = tempString[pos];
        }
        string res = "";
        foreach (var t in tempRes)
        {
            res += SufArrayDic.InvDic[(short)t];
        }
        return res;

    }

    public class MTF2
    {
        public List<Tuple<int, char>> Table = new List<Tuple<int, char>>();
        public SortedList<char, int> PosTable = new SortedList<char, int>();
        private string RawString = "";
        public List<int> Output = new List<int>();
        public List<int> RawData = new List<int>();
        public MTF2(string s)
        {
            RawString = s;
            Init();
        }

        public MTF2(List<int> input)
        {
            RawData = input;
            Init();
        }

        private void Init()
        {
            var temp = new List<Tuple<int, char>>();
            foreach (var d in SufArrayDic.Dic)
            {
                temp.Add(new Tuple<int, char>(d.Value, d.Key));
            }
            foreach (var t in temp.OrderBy(x => x.Item1).ToList())
            {
                Table.Add(t);
                PosTable.Add(t.Item2, t.Item1);
            }
        }
        public void Encode()
        {
            int prev = 1;
            foreach (var c in RawString)
            {
                int to = 0;
                int from = PosTable[c];
                if (from == 1)
                {
                    if (prev != 0)
                    {
                        //MTF
                        to = 0;
                    }
                    else
                    {
                        Output.Add(from);
                        prev = from;
                        continue;
                        //sonomama
                    }
                }
                else if (from > 1)
                {
                    //MT2
                    to = 1;
                }
                else
                {
                    Output.Add(from);
                    prev = from;
                    continue;
                    //sonomama
                }
                var temp = Table[from];
                Table.RemoveAt(from);
                PosTable[c] = to;
                var newitem = new Tuple<int, char>(to, temp.Item2);
                Table.Insert(to, newitem);
                for (int i = to + 1; i <= from; i++)
                {
                    var temp2 = Table[i];
                    Table[i] = new Tuple<int, char>(i, temp2.Item2);
                    PosTable[temp2.Item2] = i;
                }
                Output.Add(from);
                prev = from;
            }
        }

        public string Decode()
        {
            StringBuilder sb = new StringBuilder();
            int prev = 1;
            foreach (var c in RawData)
            {
                int to = 0;
                int from = Table[c].Item1;
                if (from == 1)
                {
                    if (prev != 0)
                    {
                        //MTF
                        to = 0;
                    }
                    else
                    {
                        sb.Append(Table[c].Item2);
                        prev = from;
                        continue;
                        //sonomama
                    }
                }
                else if (from > 1)
                {
                    //MT2
                    to = 1;
                }
                else
                {
                    sb.Append(Table[c].Item2);
                    prev = from;
                    continue;
                    //sonomama
                }
                var temp = Table[c];
                Table.RemoveAt(c);
                PosTable[temp.Item2] = to;
                var newitem = new Tuple<int, char>(to, temp.Item2);
                Table.Insert(to, newitem);
                for (int i = to + 1; i <= from; i++)
                {
                    var temp2 = Table[i];
                    Table[i] = new Tuple<int, char>(i, temp2.Item2);
                    PosTable[temp2.Item2] = i;
                }
                sb.Append(temp.Item2);
                prev = from;
            }
            return sb.ToString();
        }
    }

    public class PackBits
    {
        public List<int> RawData = null;
        public List<int> Output = new List<int>();
        public int Offset = 128;
        public PackBits(List<int> input, int offset = 128)
        {
            Offset = offset;
            RawData = input;
        }

        public void Encode()
        {
            if (RawData == null) return;
            for (int i = 0; i < RawData.Count; i++)
            {
                int o = i;
                if (i < RawData.Count - 1 && RawData[i] == RawData[i + 1])
                {
                    int count = 1;
                    while (i < RawData.Count - 1 && RawData[i] == RawData[i + 1])
                    {
                        i++;
                        count++;
                    }
                    while (count > 0)
                    {
                        if (count > Offset)
                        {
                            Output.Add(0);
                            Output.Add(RawData[o] + Offset);
                        }
                        else
                        {
                            Output.Add(count * -1 + Offset);
                            Output.Add(RawData[o] + Offset);
                        }
                        count -= Offset;
                    }
                }
                //エッジケース
                else if (i == RawData.Count - 1)
                {
                    Output.Add(1 + Offset);
                    Output.Add(RawData[o] + Offset);
                }
                else
                {
                    int count = 0;
                    while (i < RawData.Count - 1 && RawData[i] != RawData[i + 1] && count < Offset)
                    {
                        i++;
                        count++;
                    }
                    i--;
                    Output.Add(count + Offset);
                    for (int j = 0; j < count; j++)
                    {
                        Output.Add(RawData[o + j] + Offset);
                    }
                }
            }
        }

        public void Decode()
        {
            if (RawData == null) return;
            int prev = -1 - Offset;
            for (int i = 0; i < RawData.Count - 1; i++)
            {
                int c = RawData[i];
                c -= Offset;
                if (c < 0)
                {
                    int rep = c * -1;
                    int chara = RawData[i + 1];
                    chara -= Offset;
                    for (int j = 0; j < rep; j++)
                    {
                        Output.Add(chara);
                    }
                    i++;
                }
                else
                {
                    int rep = c;
                    for (int j = 0; j < rep; j++)
                    {
                        i++;
                        int chara = RawData[i] - Offset;
                        Output.Add(chara);
                    }
                }
            }
        }

    }

    public class AdaptiveRangeCoder
    {
        public int MaxChar = 256;
        public int TarminateChara = 255;
        public enum CodingMode { ENCODE, DECODE };
        public CodingMode Mode = CodingMode.ENCODE;
        public ulong MAX_RANGE = 0x800000000; //35bit+1
        public ulong MIN_RAMGE = 0x40000000; // 30bit+1
        public ulong MASK = 0x7ffffffff; // 35bit
        public int SHIFT = 30;
        public ulong Range = 0UL;
        public ulong Buff = 0UL;
        public ulong Low = 0UL;
        public int Ffcnt = 0;
        public Freq FreqDic = null;
        public List<int> Output;
        private string RawString = "";
        private List<int> RawData = null;
        private int _dataPtr = 0;
        public AdaptiveRangeCoder(List<int> input, CodingMode mode, int maxChar = 256)
        {
            MaxChar = maxChar;
            TarminateChara = maxChar - 1;
            Mode = mode;
            RawData = input;
            RawData.Add(TarminateChara);
            FreqDic = new Freq(maxChar, MIN_RAMGE);
            if (mode == CodingMode.ENCODE)
            {
                Low = 0UL;
                Range = MAX_RANGE;
            }
            else if (mode == CodingMode.DECODE)
            {
                if (RawData.Count < 8) return;
                Low = 0UL;
                //先頭は読み捨て
                Low = (ulong)RawData[1];
                Low = ((Low << 5) + (ulong)RawData[2]);
                Low = ((Low << 5) + (ulong)RawData[3]);
                Low = ((Low << 5) + (ulong)RawData[4]);
                Low = ((Low << 5) + (ulong)RawData[5]);
                Low = ((Low << 5) + (ulong)RawData[6]);
                Low = ((Low << 5) + (ulong)RawData[7]);
                Range = MAX_RANGE;
                _dataPtr = 8;
            }
            Output = new List<int>();
        }

        public void EncodeString()
        {
            if (RawString == "") return;
            foreach (char c in RawString)
            {
                EncodeIter(c);
            }
            EncodeFinal();
        }
        public void EncodeData()
        {
            if (RawData == null) return;
            foreach (int d in RawData)
            {
                EncodeIter(d);
            }
            EncodeFinal();
        }
        public void EncodeIter(char c)
        {

            if (SufArrayDic.Dic.ContainsKey(c))
            {
                int d = SufArrayDic.Dic[c];
                EncodeIter(d);
            }
        }
        public void EncodeIter(int d)
        {
            double temp = (double)Range / (double)FreqDic.TtlCount;
            Low += (ulong)((double)FreqDic.Cumul(d) * temp);
            Range = (ulong)((double)FreqDic.Frec[d] * temp);
            EncodeNormalize();
            FreqDic.Update(d);
        }

        public void EncodeNormalize()
        {
            if (Low >= MAX_RANGE)
            {//Lowは2倍以上にならない
                Buff += 1UL;
                Low &= MASK;
                if (Ffcnt > 0)
                {
                    //(古い)バッファをフラッシュする
                    Output.Add((int)Buff);
                    for (int i = 0; i < Ffcnt - 1; i++)
                    {
                        Output.Add(0);
                    }
                    Ffcnt = 0; // リセット
                    Buff = 0UL;
                }
            }
            while (Range < MIN_RAMGE)
            {
                if (Low < (0x1fUL << SHIFT))
                {
                    //(古い)バッファをフラッシュする
                    Output.Add((int)Buff);
                    for (int i = 0; i < Ffcnt; i++)
                    {
                        Output.Add(0x1f);
                    }
                    Buff = (Low >> SHIFT) & 0x1fUL;
                    Ffcnt = 0;
                }
                else
                {
                    Ffcnt += 1;
                }
                Low = (Low << 5) & MASK;
                Range <<= 5;
            }
        }

        public void EncodeFinal()
        {
            ulong c = 0x1fUL;
            if (Low >= MAX_RANGE)
            {
                Buff += 1UL;
                c = 0UL;
            }
            Output.Add((int)Buff);
            for (int i = 0; i < Ffcnt; i++)
            {
                Output.Add((int)c);
            }
            Output.Add((int)((Low >> 30) & 0x1fUL));
            Output.Add((int)((Low >> 25) & 0x1fUL));
            Output.Add((int)((Low >> 20) & 0x1fUL));
            Output.Add((int)((Low >> 15) & 0x1fUL));
            Output.Add((int)((Low >> 10) & 0x1fUL));
            Output.Add((int)((Low >> 5) & 0x1fUL));
            Output.Add((int)(Low & 0x1fUL));
        }
        public void DecodeData()
        {
            if (RawData == null) return;
            while (DecodeIter())
            {
            }
            if (Output.Count > 0) Output.RemoveAt(Output.Count - 1);
        }
        public bool DecodeIter()
        {
            double temp = (double)Range / (double)FreqDic.TtlCount;
            var code = SearchCode((double)Low / temp);
            int chara = code.Item1;
            ulong num = code.Item2;
            Low -= (ulong)(num * temp);
            Range = (ulong)((double)FreqDic.Frec[chara] * temp);
            DecodeNormalize();
            FreqDic.Update(chara);
            Output.Add(chara);
            if (chara == TarminateChara) return false;
            else return true;
        }

        public void DecodeNormalize()
        {
            while (_dataPtr < RawData.Count && Range < MIN_RAMGE)
            {
                Range <<= 5;
                Low = ((Low << 5) + (ulong)RawData[_dataPtr++]) & MASK;
            }
        }

        private Tuple<int, ulong> SearchCode(double value)
        {
            ulong intval = (ulong)(value + 0.0001);
            ulong sum = 0;
            for (int i = 0; i < FreqDic.MaxChar; i++)
            {
                if (intval < sum + FreqDic.Frec[i]) return new Tuple<int, ulong>(i, sum);
                sum += FreqDic.Frec[i];
            }
            return new Tuple<int, ulong>(-1, 0);
        }
        public class Freq
        {
            public int MaxChar = 256;
            public SortedList<int, ulong> Frec = new SortedList<int, ulong>();
            public ulong TtlCount = 0UL;
            public ulong MIN_RANGE = 0UL;

            public Freq(int maxChar, ulong min_range)
            {
                MIN_RANGE = min_range;
                MaxChar = maxChar;
                for (int i = 0; i < MaxChar; i++)
                {
                    Frec[i] = 1;
                }
                TtlCount = (ulong)MaxChar;
            }

            public ulong Cumul(int c)
            {
                ulong res = 0;
                for (int i = 0; i < c; i++)
                {
                    res += Frec[i];
                }
                return res;
            }
            public void Update(int d)
            {
                Frec[d]++;
                TtlCount++;
                if (TtlCount >= MIN_RANGE)
                {
                    ulong counter = 0UL;
                    for (int i = 0; i < MaxChar; i++)
                    {
                        Frec[i] = (Frec[i] >> 1) | 1UL;
                        counter += Frec[i];
                    }
                    TtlCount = counter;
                }
            }
        }
    }
    public static class CodeBook
    {
        public static SortedList<string, char> Currency = new SortedList<string, char>();
        public static SortedList<string, char> Unit = new SortedList<string, char>();
        public static SortedList<string, char> TradeTerm = new SortedList<string, char>();
        public static SortedList<string, char> OL = new SortedList<string, char>();
        static CodeBook()
        {
            Currency.Add("JPY", '1');
            Currency.Add("USD", '2');
            Currency.Add("SEK", '3');
            Currency.Add("EUR", '4');
            Currency.Add("GBP", '5');
            Currency.Add("CAD", '6');
            Currency.Add("PLN", '7');
            Currency.Add("DKK", '8');
            Currency.Add("NOK", '9');
            Currency.Add("AUD", 'A');
            Currency.Add("CHF", 'B');
            Currency.Add("CNY", 'C');
            Currency.Add("HKD", 'D');
            Currency.Add("INR", 'E');
            Currency.Add("KRW", 'F');
            Currency.Add("MYR", 'G');
            Currency.Add("MXN", 'H');
            Currency.Add("NZD", 'I');
            Currency.Add("PHP", 'J');
            Currency.Add("SGD", 'K');
            Currency.Add("TWD", 'L');
            Currency.Add("THB", 'M');
            Currency.Add("ZAR", 'N');
            Currency.Add("CZK", 'O');
            Currency.Add("IDR", 'P');
            Currency.Add("TRY", 'Q');
            Currency.Add("RUB", 'R');
            Currency.Add("BRL", 'S');
            Currency.Add("HUF", 'T');
            Currency.Add("ARS", 'U');
            Currency.Add("COP", 'V');
            Currency.Add("CLP", 'W');
            Currency.Add("KWD", 'Z');
            Unit.Add("KG", '1');
            Unit.Add("KGIC", '2');
            Unit.Add("KGII", '3');
            Unit.Add("KGDW", '4');
            Unit.Add("KGMC", '5');
            Unit.Add("GR", '6');
            Unit.Add("GRIC", '7');
            Unit.Add("GRII", '8');
            Unit.Add("GRDW", '9');
            Unit.Add("GRMC", 'A');
            Unit.Add("MT", 'B');
            Unit.Add("MTIC", 'C');
            Unit.Add("MTII", 'D');
            Unit.Add("MTDW", 'E');
            Unit.Add("MTMC", 'F');
            Unit.Add("L", 'G');
            Unit.Add("KL", 'H');
            Unit.Add("CC", 'I');
            Unit.Add("CM", 'J');
            Unit.Add("SC", 'K');
            Unit.Add("SM", 'L');
            Unit.Add("M", 'O');
            Unit.Add("NO", 'P');
            Unit.Add("ST", 'Q');
            Unit.Add("PR", 'R');
            Unit.Add("GS", 'S');
            Unit.Add("DZ", 'T');
            Unit.Add("TH", 'U');
            TradeTerm.Add("FOB", '1');
            TradeTerm.Add("C&I", '2');
            TradeTerm.Add("C&F", '3');
            TradeTerm.Add("CIF", '4');
            TradeTerm.Add("EXW", '5');
            TradeTerm.Add("FCA", '6');
            TradeTerm.Add("FAS", '7');
            TradeTerm.Add("DAP", '8');
            TradeTerm.Add("DAF", '9');
            TradeTerm.Add("DES", 'A');
            TradeTerm.Add("DDU", 'B');
            TradeTerm.Add("DPU", 'C');
            TradeTerm.Add("DAT", 'D');
            TradeTerm.Add("DEQ", 'E');
            TradeTerm.Add("DDP", 'F');
            TradeTerm.Add("CFR", 'G');
            TradeTerm.Add("CPT", 'H');
            TradeTerm.Add("CIP", 'I');
            OL.Add("AN", '1');
            OL.Add("PL", '2');
            OL.Add("NA", '3');
        }
    }
}
public class ListWFindMax<T> :
            List<T> where T : IComparable
{
    public ListWFindMax() : base() { }

    public ListWFindMax(List<T> collection) : base()
    {
        foreach (var item in collection)
        {
            this.Add(item);
        }
    }

    public new void Add(T item)
    {
        var idx = FindMax(item);
        this.Insert(idx + 1, item);
    }

    public int FindMax(T key)
    {
        if (this.Count < 1) return -1;
        //T以下で最も大きい要素をindexで返す
        return findMax_iter(key, 0, this.Count - 1);

    }
    private int findMax_iter(T key, int s, int e)
    {
        if (s == e)
        {
            if (this.ElementAt(s).CompareTo(key) < 0)
            {
                return s;
            }
            else
            {
                return -1;
            }
        }
        int d = s + (e - s) / 2;
        if (this.ElementAt(d).CompareTo(key) == 0)
        {
            while (d < this.Count - 1 && this.ElementAt(d + 1).CompareTo(this.ElementAt(d)) == 0) d++;
            return d;
        }
        else
        {
            if (this.ElementAt(d).CompareTo(key) < 0)
            {
                return findMax_iter(key, d + 1, e);
            }
            else
            {
                return findMax_iter(key, s, d - 1);
            }
        }

    }
}