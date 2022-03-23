using System;
using System.Text;
using System.Net;
using System.IO;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Linq;

namespace AppleAPIConsoleApp
{
    /// <summary>
    /// Class for Handling API Request
    /// </summary>
    public class APIRequester
    {
        private readonly string apiurl;

        public APIRequester(string apiurl)
        {
            this.apiurl = apiurl;
        }

        /// <summary>
        /// Method which executes the user web page request and returns an http response object
        /// </summary>
        /// <param name="id">Id to be passed to the api</param>
        /// <param name="fieldName">fieldName is the key of the json value we want</param>
        /// <returns></returns>
        public string getFieldValueFromJson(string id, string fieldName)
        {
            JObject jsonData = null;
            string res = String.Empty;

            //Check if the arguments supplied are null or empty if not throw an exception
            if (String.IsNullOrEmpty(id) || String.IsNullOrEmpty(fieldName))
            {
                throw new ArgumentException("Invalid arguments please supply a valid id or fieldName");
            }

            //Call the getFullApiJsonDataWithId method to get the full json result and
            //Desirialize the result to be casted to a JObject.
            try
            {
                jsonData = (JObject)JsonConvert.DeserializeObject(getFullApiJsonDataWithId(id));

            }catch(Exception e)
            {
                throw new Exception(e.Message);
            }

            
            //iterate through the jsonData result
            foreach (var current in jsonData)
            {
                var currentValue = current.Value;

                //If the key is equal to the fieldName we supplied
                // we set res to the value and return the value from the json
                if (fieldName.CompareTo(current.Key) == 0)
                {
                    res = currentValue.ToString();
                }               
                else if (currentValue.GetType() == typeof(JArray))
                {   //If the type of currentValue is a JArray then we try to find the
                    //fieldName from the children of the JArray
                    try
                    {
                        res = currentValue.Children()[fieldName].First().ToString();
                    }
                    catch (Exception)
                    {
                        continue;
                    }
                }

                // Terminate the loop once we found the value we need
                if(!String.IsNullOrEmpty(res))
                {
                    break;
                }
            }

            // Verify that the result is not null or empty
            if (String.IsNullOrEmpty(res))
            {
                throw new APINoResultException("API didn't return any results.");
            }

            return res;
        }

        /// <summary>
        /// Method which executes the user web page request and returns an http response object
        /// </summary>
        /// <param name="pageURL"></param>
        /// <returns></returns>
        private string getFullApiJsonDataWithId(string id)
        {
            
            string fullURL = apiurl + id;

            //String variable to store the data later
            string data = string.Empty;

            //Call the isValidURL method to check if URL is valid
            if (fullURL != null)
            {
                try
                {
                    //Ceate a web request a store it in a variable
                    HttpWebRequest request = (HttpWebRequest)WebRequest.Create(fullURL);
                    //Store the response of the request
                    HttpWebResponse response = (HttpWebResponse)request.GetResponse();

                    //Do a check if status code is OK
                    if (response.StatusCode == HttpStatusCode.OK)
                    {
                        
                        //Get the response stream and store it in a variable
                        using (Stream receiveStream = response.GetResponseStream())
                        {
                            StreamReader readStream = null;

                            if (String.IsNullOrWhiteSpace(response.CharacterSet))
                            {
                                readStream = new StreamReader(receiveStream);
                            }
                            else
                            {
                                readStream = new StreamReader(receiveStream, Encoding.GetEncoding(response.CharacterSet));
                            }

                            //Read the stream and store in data variable
                            data = readStream.ReadToEnd();
                            response.Close();
                        }
                    }
                }
                catch (Exception e)
                {
                    throw new Exception(e.Message);
                }
            }

            //Check that the API returned a value
            if (String.IsNullOrEmpty(data))
            {
                throw new APINoResultException("API didn't return any results.");
            }

            return data;
        }
    }

}
