using System.Collections.Generic;
namespace MoogleEngine;
public class CreateSnippet
{ public static string Snippet(float[]Weight_Q,int Docnum)//crear nuevo metodo...
    {
     float[]WQB = TF_IDF.DotProduct(Weight_Q,Docnum);
    float max = 0.00000001F ;
    int Pos = -1;
    for (int i = 0; i < WQB.Length; i++)
    { if (WQB[i]==0)
    {
      continue;
    }
      if(WQB[i]>max)
       { max = WQB[i];
      
         Pos = i;} 

         
    }
    int start;
    int end;
 
    if (Pos>=0)
    {
     int Place = FirstPosinDoc(Database.Words[Pos],Docnum);
    
   if (Place >15)
   {start = Place-15;
      
   }else{start = Place;}
   if (Place < (Database.Documents[Docnum].Length-15))
   {
    end = Place +15;   
   }else{end = Place;}
   string Snippet=" ";
  for (int i = start; i <= end; i++)
  {
   Snippet+=' '+Database.Documents[Docnum][i];  
  }
  return Snippet;
  }else{return"no hay coincidencias";}
 }
  public static int FirstPosinDoc(string word,int Docnum)//Finds first appearance of word
      {
        int Pos = 0; 
        for (int i = 0;i <Database.Documents[Docnum].Length ; i++)
        {
          if (word ==Database.Documents[Docnum][i])
          {
             Pos = i;
             break;
          }
        }
        return Pos;
      }
}
 