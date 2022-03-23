using System;

namespace AppleAPIConsoleApp
{
    class Program
    {
        static void Main(string[] args)
        {
            
            APIRequester requester = new APIRequester("https://itunes.apple.com/lookup?id=");

            try
            {
                //1436151665
                Console.WriteLine(requester.getFieldValueFromJson("1353551752", "bundleId"));

                //1436151665
                Console.WriteLine(requester.getFieldValueFromJson("1436151665", "bundleId"));

                //1043639756
                Console.WriteLine(requester.getFieldValueFromJson("1043639756", "bundleId"));

                //Valid fieldName with non-existent id
                //Console.WriteLine(requester.getFieldValueFromJson("104363975612312", "bundleId"));

                //Valid id with non-existent fieldName
                //Console.WriteLine(requester.getFieldValueFromJson("1043639756", "bundleI"));

                //Invalid arguments
                //Console.WriteLine(requester.getFieldValueFromJson("", ""));
                //Console.WriteLine(requester.getFieldValueFromJson(null, null));
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }

        }
    }
}
