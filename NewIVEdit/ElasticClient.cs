using System;
using Nest;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using NewIVEdit;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;


public class IVEditElasticClient
{
    private static ElasticClient client = null;
    private static Uri _elasticServiceHost = new Uri("http://localhost:9200/");
    public Uri ElasticServiceHost {
        set
        {
            _elasticServiceHost = value;
            var _setting = new ConnectionSettings(_elasticServiceHost).DefaultMappingFor<CompanyProfile>(m => m
            .IndexName("company_profiles")
            );
            client = new ElasticClient(_setting);
        }
        get
        {
            return _elasticServiceHost;
        }
    }
    public IVEditElasticClient()
    {
        var _setting = new ConnectionSettings(ElasticServiceHost);
        client = new ElasticClient(_setting);
    }

    public class CompanyIndex : IComparable<CompanyIndex>, IIndexed, INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        private void NofityPropertyChanged([CallerMemberName] String propertyName = "")
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }
        }

        public int IndexNo { set; get; }
        public int CompareTo(CompanyIndex that)
        {
            return String.Compare(that.organizationno, organizationno);
        }

        public int Compare(CompanyIndex _this, CompanyIndex that)
        {
            return String.Compare(that.organizationno, _this.organizationno);
        }
        public string Id { set; get; }
        public string organizationno { set; get; }
        public string name { set; get; }
        public ObservableCollection<Subtype> subtype { set; get; }

        public class Subtype : IComparable<Subtype>, IIndexed, INotifyPropertyChanged
        {
            public event PropertyChangedEventHandler PropertyChanged;

            private void NofityPropertyChanged([CallerMemberName] String propertyName = "")
            {
                if (PropertyChanged != null)
                {
                    PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
                }
            }

            public int IndexNo { set; get; }
            public int CompareTo(Subtype that)
            {
                return String.Compare(that.ref_id, ref_id);
            }

            public int Compare(Subtype _this, Subtype that)
            {
                return String.Compare(that.ref_id, _this.ref_id);
            }
            public string key { set; get; }
            public string description { set; get; }
            public string ref_id { set; get; }
            public string hsmaster { set; get; }

            public CompanyProfile ref_prof = null;

        }
    }

    public class UpdateCompanyIndexClass
    {
        public string Id { set; get; }
        public string organizationno { set; get; }
        public string name { set; get; }
        public List<UpdateSubtypeClass> subtype { set; get; }
        public class UpdateSubtypeClass
        {
            public string key { set; get; }
            public string description { set; get; }
            public string ref_id { set; get; }
            public string hsmaster { set; get; }

        }
    }

    public class CompanyProfile : IComparable<CompanyProfile>, IIndexed, INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        private void NofityPropertyChanged([CallerMemberName] String propertyName = "")
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }
        }

        public int IndexNo { set; get; }
        public int CompareTo(CompanyProfile that)
        {
            return String.Compare(that._id, _id);
        }

        public int Compare(CompanyProfile _this, CompanyProfile that)
        {
            return String.Compare(that._id, _this._id);

        }
        public string _id { set; get; }
        public Clue_Profile[] declelementocr { set; get; }
        public bool is_serialized { set; get; } = false;
        public bool multiple_excel_sheets { set; get; } = true;
        public DataCoord[] datacoord { set; get; }
        public Converter[] converter { set; get; }
        public string existby { set; get; }
        public string existby_secondary { set; get; }
        public string invnoby { set; get; }
        public string amtby { set; get; }
        public string numberby { set; get; }
        public string netwtby { set; get; }
        public string orby { set; get; }
        public string curby { set; get; }
        public string descby { set; get; }
        public string hsby { set; get; }
        public string hssuggestby { set; get; }
        public string hsdeterminationtype { set; get; }
        public string hssuggesttype { set; get; }
        public int determinationfuzzylevel { set; get; } = 0;
        public int suggestionfuzzylevel { set; get; } = 0;
        public string packing_existby { set; get; }
        public string packing_existby_secondary { set; get; }
        public string packing_invnoby { set; get; }
        public string packing_amtby { set; get; }
        public string packing_numberby { set; get; }
        public string packing_netwtby { set; get; }
        public string packing_orby { set; get; }
        public string packing_curby { set; get; }
        public string packing_descby { set; get; }
        public string packing_hsby { set; get; }
        public string packing_hssuggestby { set; get; }
        public string packing_hsdeterminationtype { set; get; }
        public string packing_hssuggesttype { set; get; }
        public int packing_determinationfuzzylevel { set; get; }
        public int packing_suggestionfuzzylevel { set; get; }
        public string description { set; get; }
        public bool hs_forward { set; get; } = false;
        public bool hs_backward { set; get; } = false;
        public bool hs_readonly { set; get; } = true;
        public bool hs_isnormalize { set; get; } = true;
        public bool hs_enablesuggestion { set; get; } = false;
        public bool enablepagekeyword { set; get; }
        public float keywordx { set; get; }
        public float keywordy { set; get; }
        public float keywordw { set; get; }
        public float keywordh { set; get; }
        public string keywordpattern { set; get; }
        public string defaulttradeterm { set; get; }
        public bool enable_knockout { set; get; } = false;
        public string knockout_before { set; get; }
        public string knockout_after { set; get; }

        public ValidPatterns valid_patterns { set; get; }
        public Clue_Profile[] clue_profiles { set; get; }
        public Data_Const[] data_const { set; get; }
        public Data_Register[] data_register { set; get; }
        public Data_Bucket[] data_bucket { set; get; }
        public Side_Channel[] side_channel { set; get; }

    }
    public class ValidPatterns
    {
        public string exist { set; get; }
        public string exist_secondary { set; get; }

        public string invn { set; get; }
        public string amount { set; get; }
        public string number { set; get; }
        public string netweight { set; get; }
        public string origin { set; get; }
        public string hskey { set; get; }
        public string hsclue { set; get; }
        public string currency { set; get; }
        public string description { set; get; }

    }
    public class DataCoord
    {
        public string label { set; get; }
        public string mode { set; get; }
        public bool isexcel { set; get; }
        public bool is_backward { set; get; } = false;
        public string start_pattern { set; get; }
        public bool re_ocr { set; get; }
        public string start_clue { set; get; }
        public string end_clue { set; get; }
        public string ref_clue { set; get; }
        public float from { set; get; }
        public float to { set; get; }
        public bool is_completematch { set; get; }
        public string prefix_pattern { set; get; }
        //fuzzy serch should be implemented
        public string pattern { set; get; }
        public string postfix_pattern { set; get; }
        public string knockout_prefix_pattern { set; get; }
        public string knockout_pattern { set; get; }
        public string knockout_postfix_pattern { set; get; }
        public int fuzzy_level { set; get; }
        public int offset { set; get; } = 0;

    }

    public class Clue_Profile
    {
        public string label { set; get; }
        public string mode { set; get; }
        public bool is_excel { set; get; }
        public bool re_ocr { set; get; }
        public float x { set; get; }
        public float y { set; get; }
        public float w { set; get; }
        public float h { set; get; }
        public bool is_completematch { set; get; }

        public string prefix_pattern { set; get; }
        public string pattern { set; get; }

        public string postfix_pattern { set; get; }
        //fuzzy serch should be implemented
        public int fuzzy_level { set; get; }

    }

    public class Data_Bucket
    {
        public string description { set; get; }
        public string label { set; get; }
        public string type { set; get; }
        public string target { set; get; }
        public string partitioned_by { set; get; }
    }

    public class Data_Const
    {
        public string description { set; get; }
        public string label { set; get; }
        public string value { set; get; }
    }
    public class Data_Register
    {
        public string description { set; get; }
        public string label { set; get; }
        public string type { set; get; }
    }

    public class Side_Channel
    {
        public string description { set; get; }
        public string label { set; get; }
        public string key_field { set; get; }
        public bool is_autogenerate { set; get; }
        public Field[] field { set; get; }
        public SideChannel_Converter[] converter { set; get; }

        public class Field
        {
            public string description { set; get; }
            public string label { set; get; }
            public string default_value { set; get; }

        }
    }

    public class Converter
    {
        public string description { set; get; }
        public string condition_field { set; get; }
        public string condition_type { set; get; }
        public string condition_right_operand_factor { set; get; }
        public string action_operator { set; get; }
        public string action_left_operand_field { set; get; }
        public string action_right_operand_field { set; get; }
        public string action_left_operand_factor { set; get; }
        public string action_right_operand_factor { set; get; }
        public string result_field { set; get; }
        public bool isLUA { set; get; }
        public string LUAScript { set; get; }

    }

    public class SideChannel_Converter
    {
        public string description { set; get; }
        public string condition_sidechannel_name { set; get; }
        public string condition_sidechannel_field { set; get; }
        public string condition_type { set; get; }
        public string condition_right_operand_field { set; get; }
        public string action_operator { set; get; }
        public string action_left_operand_field { set; get; }
        public string action_right_operand_field { set; get; }
        public string result_field { set; get; }
        public bool isLUA { set; get; }
        public string LUAScript { set; get; }

    }

    public class HSMaster
    {
        public string productioncode { set; get; }

        public string threegramproductioncode {set;get;}
        public string wildcardproductioncode { set; get; }

        public string update_time { set; get; }

        public string hscode { set; get; }

        public bool is_outdated { set; get; } = false;

    }

    public class UpdateHSMaster
    {
        public string productioncode { set; get; }
        public string threegramproductioncode { set; get; }
        public string wildcardproductioncode { set; get; }

        public string update_time { set; get; }

        public string hscode { set; get; }
        public bool is_outdated { set; get; } = false;
    }
    public List<CompanyIndex> GetCompanyIndicies()
    {
        var searchResponse = client.Search<CompanyIndex>(s => s
        .Index("company_indicies")
            .Size(10000)
            .Query(q => q
                .MatchAll(m => m
                )
            )
        );
        var companies = searchResponse.Documents;
        var res = searchResponse.Hits.Select(h =>
        {
            h.Source.Id = h.Id;
            return h.Source;
        }).ToList();

        return res;
    }
    public static CompanyProfile GetCompanyProfile(string id)
    {
        var searchResponse = client.Search<CompanyProfile>(s => s
        .Index("company_profiles")
            .Query(q => q
                .Match(m => m
                    .Field(i => i._id)
                        .Query(id)
                )
            )
        );
        var companies = searchResponse.Documents; 
        var res = searchResponse.Hits.Select(h =>
            {
                h.Source._id = id;
                return h.Source;
            }).ToList();
        if (res != null && res.Count() > 0) return res[0];
        else return null;
    }

    public static bool CheckIfClientValid()
    {
        return client.Ping().IsValid;
    }

    public static bool CheckIfIndexExist(string indexname)
    {
        return client.Indices.Exists(indexname).Exists;
    }

    public static bool CreateIndex(string indexname)
    {
        var res = client.Indices.Create(indexname,
        index => index
        .Settings
            (
                setting => setting.Analysis(
                    an =>
                    an.Tokenizers(
                            tk => tk.NGram("2gram_tokenizer",
                                th => th.MinGram(1).
                                MaxGram(2)
                                .TokenChars(Nest.TokenChar.Digit, Nest.TokenChar.Letter)
                            )
                        ).CharFilters(
                            ch => ch.Mapping("replace_whitespace",
                                f => f.Mappings("\\u0020=>", "\\u3000=>")
                            )
                        ).Normalizers(
                            no => no.Custom("replace_whitespace_norm",
                                n => n.CharFilters("replace_whitespace")
                                    .Filters("lowercase")
                                )
                        ).Analyzers
                            (
                            term_joint => term_joint.
                                Custom("term_joint",
                                    x => x.CharFilters("replace_whitespace")
                                    .Filters("lowercase")
                                    .Tokenizer("standard")
                                ).Custom("2gram",
                                    y => y.Tokenizer("2gram_tokenizer")
                                    .Filters("lowercase")
                                        )
                            )
                        )
            )
        .Map<HSMaster>(m => m
            .Properties(p => p
                    .Text(prod => prod
                        .Name(n => n.productioncode)
                        .Analyzer("term_joint")
                        )
                    .Text(t => t
                        .Name(n => n.threegramproductioncode)
                        .Analyzer("2gram")
                    ).Keyword(w => w
                        .Name(n => n.wildcardproductioncode)
                        .Normalizer("replace_whitespace_norm")
                    ).Keyword(k => k
                        .Name(n => n.hscode)
                    ).Date(dt => dt
                        .Name(n => n.update_time)
                    ).Boolean(b => b
                        .Name(outdate => outdate.is_outdated)
                    )
                )
            )
        ) ;
        return res.IsValid;
    }

    public static void UpdateCompanyIndex(CompanyIndex index)
    {
        var tempSubtype = new List<UpdateCompanyIndexClass.UpdateSubtypeClass>();
        foreach(var s in App.MainView.CompanySubtypes)
        {
            tempSubtype.Add(new UpdateCompanyIndexClass.UpdateSubtypeClass()
            {
                key = s.key,
                description = s.description,
                ref_id = s.ref_id,
                hsmaster = s.hsmaster,
            });
        }
        var tempIndex = new UpdateCompanyIndexClass() {
            Id = index.Id,
            organizationno = index.organizationno,
            name = index.name,
            subtype = tempSubtype,
        };

        var temp = client.Update<UpdateCompanyIndexClass, object>(tempIndex.Id, p => p
            .Index("company_indicies")
            .Doc(tempIndex));
    }

    public static string GetHSCode(string indexname, string prodcode,bool isForward, bool isBackward, bool isNormalize = true) {

        string res = "";
        string pattern = prodcode;
        ISearchResponse<HSMaster> searchResponse;
        if (!isForward && !isBackward)
        {
            if (isNormalize)
            {
                searchResponse = client.Search<HSMaster>(s => s
                .Index(indexname)
                    .Query(q => q
                        .Term(
                            w => w.Field("wildcardproductioncode")
                            .Value(pattern)
                            )
                        )
                        .Aggregations(aggs => aggs
                            .TopHits("latest", tops => tops
                                .Size(1)
                                .Sort(srt => srt
                                    .Descending(f => f.update_time)
                                )
                        )
                    )
                );
            }
            else
            {
                searchResponse = client.Search<HSMaster>(s => s
                .Index(indexname)
                    .Query(q => q
                        .Match(
                            w => w.Field("productioncode")
                                .Query(pattern)
                                .Analyzer("term_joint")
                            )
                        )
                        .Aggregations(aggs => aggs
                            .TopHits("latest", tops => tops
                                .Size(1)
                                .Sort(srt => srt
                                    .Descending(f => f.update_time)
                                )
                        )
                    )
                );
            }
        }
        else
        {
            if (isForward) pattern += @"*";
            if (isBackward) pattern = @"*" + pattern;
            searchResponse = client.Search<HSMaster>(s => s
            .Index(indexname)
                .Query(q => q
                    .Wildcard(
                        w => w.Field("wildcardproductioncode")
                        .Value(pattern)
                        )
                    )
                    .Aggregations(aggs => aggs
                        .TopHits("latest", tops => tops
                            .Size(1)
                            .Sort(srt => srt
                                .Descending(f => f.update_time)
                            )
                    )
                )
            );
        }
        if (!searchResponse.IsValid) return "";
        var aggres = searchResponse.Aggregations.TopHits("latest").Hits<HSMaster>();
        if(aggres.Count > 0)
        {
            res = aggres.ElementAt(0).Source.hscode;
        }
        return res != null? res : "";
    }

    public static string GetHSSuggestion(string indexname, string prodcode)
    {

        string res = "";
        string pattern = prodcode;
        ISearchResponse<HSMaster> searchResponse =client.Search<HSMaster>(s => s
        .Index(indexname)
            .Size(1)
            .Query(q => q
                .Match(
                    w => w.Field("threegramproductioncode")
                        .Query(pattern)
                        .Analyzer("2gram")
                    )
                ).PostFilter(pf=>
                    pf.Term(t=>
                        t.Field("is_outdated")
                            .Value(false)
                        )
                )
        );
        var aggres = searchResponse.Hits;
        if (aggres.Count > 0)
        {
            res = aggres.ElementAt(0).Source.hscode;
        }
        return res != null ? res : "";
    }

    public static bool PostHSCode(string indexname, string prodcode,string hscode,string hsclue, DateTime updateTime)
    {
        var tempHS = new UpdateHSMaster()
        {
            wildcardproductioncode = prodcode,
            threegramproductioncode = (hsclue!= null && hsclue != "") ? hsclue : prodcode,
            productioncode = prodcode,
            hscode = hscode.Length > 9 ? hscode.Substring(0,9) : hscode,
            update_time = updateTime.ToString("s"),
        };

        var res = client.IndexAsync(tempHS,i => i.Index(indexname));
        return res.Result.IsValid;
    }
}



