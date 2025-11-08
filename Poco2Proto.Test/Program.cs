using Poco2Proto;

public class UserProfile
{
    public string FullName { get; set; }
    public int? Age { get; set; }
    public bool IsActive { get; set; }
    public List<string> Roles { get; set; }
    public string[] Tags { get; set; }
}


public class Program
{
    public static void Main()
    {
        string protoOutput = Poco2Proto.Poco2Proto.GenerateProto<UserProfile>();

        Console.WriteLine("--- Generated UserProfile.proto ---");
        Console.WriteLine(protoOutput);

       // saving the file
         File.WriteAllText("UserProfile.proto", protoOutput);
    }
}