using System.Reflection.Metadata;
using System.Linq;
namespace MoogleEngine
{
    public class Database
    {
   
    public static string[]Alltitles = Database.Titles();
    public const string FilePath = @"C:\Users\PC\Desktop\School\First project moogle-2021\Content";
    public static string[][]Documents = Database.Docs();
    public static string[]Words = Database.AllWords();
    public static int  NumofWords = Words.Length;
    public static string[][]Docs() //loads txt into a jagged array
    {   
        string[]allFiles = Directory.GetFiles(FilePath,"*.txt");//array con la direccion de cada txt
        string[]TextRaw = new string[allFiles.Length] ;
     try{
        for (int i = 0; i < allFiles.Length; i++)
        {
            using (StreamReader sr = new StreamReader(allFiles[i]))
           {   
               string line;
               while ((line= sr.ReadLine()) != null)
              {
                TextRaw[i]+=line;
              }
            }
       ; }
      
    }
    catch(Exception e)
    {//error al leer
    Console.WriteLine("el archivo no se pudo leer");
    Console.WriteLine(e.Message);}
    return Database.Separate(TextRaw);
    }
    public static string[][]Separate (string[]TextRaw)//spits the text into key words
    {string[][] AfterSplitArray = new string[TextRaw.Length][];
    char[]separators = {',' ,'.',';','+','*','/','&','%','@','!','?',':','<','>','(',')','{','}','[',']','~',' ','_','!','^','*','$','"'};
   for (int Q = 0; Q < TextRaw.Length; Q++)
   {
    AfterSplitArray[Q] = TextRaw[Q].Split(separators,StringSplitOptions.RemoveEmptyEntries & StringSplitOptions.TrimEntries);
   }
   return AfterSplitArray;
    }
    public static string[]SeparateQuery(string query)//splits query into key words
    {
        char[]separators = {',','.',';','+','*','/','&','%','@','!','?',':','<','>','(',')','{','}','[',']','~','_','!','^','*','$','"'};
      string[]Query = Array.ConvertAll(query.Split(separators,StringSplitOptions.TrimEntries & StringSplitOptions.RemoveEmptyEntries),p => p.Trim());
      return Query;
    }
    public static string[]AllWords()
    {  /* string[]AllWords = Docs[0];
        for (int i = 1; i < Docs.Length; i++)
        {
            AllWords = AllWords.Union(Docs[i]).ToArray();
        }
        return AllWords;*/
        string[]MergeQuery = Documents[0];
    for (int i = 0; i < (Documents.Length); i++)
    {
     MergeQuery= MergeQuery.Concat(Documents[i]).ToArray();   
    }
    string[]Unique = MergeQuery.Distinct().ToArray();
   Array.Sort(Unique,StringComparer.CurrentCultureIgnoreCase);
    return Unique;
    }
    public static string[] Titles()
    {   string[]allFiles = Directory.GetFiles(FilePath,"*.txt");//array con la direccion de cada txt
        string[]Titles = new string[allFiles.Length] ;
        for (int i = 0; i < Titles.Length; i++)
        {
          Titles[i]=new string(allFiles[i].Replace(FilePath,string.Empty));
          Titles[i]=Titles[i].Replace(".txt",string.Empty);
          Titles[i]=Titles[i].ToUpper();
        }
        return Titles;
    }
    public static int DocNum = Database.Documents.Length;
    }
}