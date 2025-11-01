using System.Text;

namespace HiddenTextNet.Example;

class Program
{
    static void Main(string[] args)
    {
        var str = WhitespaceText.Encode("Lorem ipsum");
        File.WriteAllText("myfile.txt", str, Encoding.UTF8);
        
        Console.WriteLine("Done");
    }
}