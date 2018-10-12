/**********************************************************************************************
 * Copyright 2009 Amazon.com, Inc. or its affiliates. All Rights Reserved.
 *
 * Licensed under the Apache License, Version 2.0 (the "License"). You may not use this file 
 * except in compliance with the License. A copy of the License is located at
 *
 *       http://aws.amazon.com/apache2.0/
 *
 * or in the "LICENSE.txt" file accompanying this file. This file is distributed on an "AS IS"
 * BASIS, WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied. See the
 * License for the specific language governing permissions and limitations under the License. 
 *
 * ********************************************************************************************
 *
 *  Amazon Product Advertising API
 *  Signed Requests Sample Code
 *
 *  API Version: 2009-03-31
 *
 */

using System;
using System.Collections.Generic;
using System.Text;
using AmazonProductAdvtApi;
using Microsoft.Web.Services3.Addressing;
using System.Security.Cryptography.X509Certificates;
using System.Net.Security;
using System.Net;

namespace AmazonProductAdvtApi
{
    class HmacSample
    {
        // Use one of the following destinations, according to the region you are
        // interested in:
        // 
        //      US: ecs.amazonaws.com 
        //      CA: ecs.amazonaws.ca 
        //      UK: ecs.amazonaws.co.uk 
        //      DE: ecs.amazonaws.de 
        //      FR: ecs.amazonaws.fr 
        //      JP: ecs.amazonaws.jp
        //
        // Note: protocol must be https for signed SOAP requests.
        const String DESTINATION    = "https://ecs.amazonaws.com/onca/soap?Service=AWSECommerceService";

        // Set your AWS Access Key ID and AWS Secret Key here.
        // You can obtain them at:
        // http://aws-portal.amazon.com/gp/aws/developer/account/index.html?action=access-key
        const String MY_AWS_ID      = "YOUR_AWS_ACCESS_KEY_ID_HERE";
        const String MY_AWS_SECRET  = "YOUR_AWS_SECRET_KEY_HERE";

        // Select an item you wish to inspect and provide it's ASIN here:
        const String ITEM_ID = "0545010225";

        static void Main(string[] args)
        {
            // If you are using a debugging proxy like Fiddler to capture SOAP
            // envelopes, and you get SSL Certificate warnings, uncomment the 
            // following line:
            // ServicePointManager.ServerCertificateValidationCallback = delegate { return true; };

            // Create an ItemLookup request, with ResponseGroup "Small"
            // and using the item ID provided above.
            ItemLookup itemLookup = new ItemLookup();
            itemLookup.AWSAccessKeyId = MY_AWS_ID;

            ItemLookupRequest itemLookupRequest = new ItemLookupRequest();
            itemLookupRequest.ItemId = new String[] { ITEM_ID };
            itemLookupRequest.ResponseGroup = new String[] { "Small", "AlternateVersions" };
            itemLookup.Request = new ItemLookupRequest[] { itemLookupRequest };

            // create an instance of the serivce
            AWSECommerceService api = new AWSECommerceService();

            // set the destination
            api.Destination = new Uri(DESTINATION);

            // apply the security policy, which will add the require security elements to the
            // outgoing SOAP header
            AmazonHmacAssertion amazonHmacAssertion = new AmazonHmacAssertion(MY_AWS_ID, MY_AWS_SECRET);
            api.SetPolicy(amazonHmacAssertion.Policy());

            // make the call and print the title if it succeeds
            try
            {
                ItemLookupResponse itemLookupResponse = api.ItemLookup(itemLookup);
                Item item = itemLookupResponse.Items[0].Item[0];
                System.Console.WriteLine(item.ItemAttributes.Title);
            }
            catch (Exception e)
            {
                System.Console.Error.WriteLine(e);
            }

            // we're done
            System.Console.WriteLine("HMAC sample finished. Hit Enter to terminate.");
            System.Console.ReadLine();
        }
    }
}
