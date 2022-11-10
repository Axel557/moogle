using System.Collections.Generic;
using System.Reflection.Metadata.Ecma335;
using System.IO;
using System.Linq;
namespace MoogleEngine;


public class Moogle
{ 
    public static SearchResult Query(string query) {
        string[]Query = Database.SeparateQuery(query);
        int[]Importance = Operators.FindOperatorofImportance(Query,query); 
        bool[]Proximity = Operators.FindProximityOperator(query,Query);
        var Weight_Q = TF_IDF.Weight_Query(Query,Importance);
        bool[]Mustbe = Operators.MustbePresent(Query,Operators.FindOperators(Query,'^',query));
        bool[]MustNOTbe =Operators.MustbeNOTPresent(Query,Operators.FindOperators(Query,'!',query));
        float[]Values = SearchItem.AllScores(Weight_Q,Mustbe,MustNOTbe,Query,Proximity);
        SearchItem[]items = SearchItem.CreateSearchItem(Values,Weight_Q);
        return new SearchResult(items, Scores.suggestion(Query));

    }
}