﻿using Microsoft.Recognizers.Text.Number.Extractors;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Text.RegularExpressions;

namespace Microsoft.Recognizers.Text.Number.English.Extractors
{
    public class OrdinalExtractor : BaseNumberExtractor
    {
        internal sealed override ImmutableDictionary<Regex, string> Regexes { get; }
        protected sealed override string ExtractType { get; } = Constants.SYS_NUM_ORDINAL; // "Ordinal";

        public const string RoundNumberOrdinalRegex = @"(hundredth|thousandth|millionth|billionth|trillionth)";

        public const string BasicOrdinalRegex =
            @"(first|second|third|fourth|fifth|sixth|seventh|eighth|ninth|tenth|eleventh|twelfth|thirteenth|fourteenth|fifteenth|sixteenth|seventeenth|eighteenth|nineteenth|twentieth|thirtieth|fortieth|fiftieth|sixtieth|seventieth|eightieth|ninetieth)";

        public static string SuffixBasicOrdinalRegex
            =>
                $@"((((({IntegerExtractor.TensNumberIntegerRegex}(\s+(and\s+)?|\s*-\s*){
                    IntegerExtractor.ZeroToNineIntegerRegex})|{IntegerExtractor
                        .TensNumberIntegerRegex}|{IntegerExtractor.ZeroToNineIntegerRegex}|{IntegerExtractor.AnIntRegex
                    })(\s+{IntegerExtractor
                        .RoundNumberIntegerRegex})+)\s+(and\s+)?)*({IntegerExtractor.TensNumberIntegerRegex
                    }(\s+|\s*-\s*))?{BasicOrdinalRegex})";

        public static string SuffixRoundNumberOrdinalRegex
            =>
                $@"(({IntegerExtractor.AllIntRegex}\s+){RoundNumberOrdinalRegex})";

        public static string AllOrdinalRegex
            =>
                $@"({SuffixBasicOrdinalRegex}|{SuffixRoundNumberOrdinalRegex})";

        public OrdinalExtractor()
        {
            var _regexes = new Dictionary<Regex, string>
            {
                {
                    new Regex(
                        @"(?<=\b)((\d*(1st|2nd|3rd|4th|5th|6th|7th|8th|9th|0th))|(11th|12th))(?=\b)",
                        RegexOptions.Compiled|RegexOptions.IgnoreCase | RegexOptions.Singleline)
                    , "OrdinalNum"
                },
                {
                    new Regex(@"(?<=\b)(\d{1,3}(\s*,\s*\d{3})*(\.\d+)?\s*th)(?=\b)",
                        RegexOptions.Compiled|RegexOptions.IgnoreCase | RegexOptions.Singleline)
                    , "OrdinalNum"
                },
                {
                    new Regex($@"(?<=\b){AllOrdinalRegex}(?=\b)", RegexOptions.Compiled|RegexOptions.IgnoreCase | RegexOptions.Singleline)
                    , "OrdEng"
                },
                {
                    new Regex($@"(?<!(a|an)\s+){RoundNumberOrdinalRegex}",
                        RegexOptions.Compiled|RegexOptions.IgnoreCase | RegexOptions.Singleline)
                    , "OrdEng"
                }
            };
            Regexes = _regexes.ToImmutableDictionary();
        }
    }
}