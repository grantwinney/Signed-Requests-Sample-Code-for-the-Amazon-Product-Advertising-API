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
using System.Net;
using System.Security.Cryptography.X509Certificates;

using Microsoft.Web.Services3.Addressing;

using AmazonProductAdvtApi;

namespace AmazonProductAdvtApi
{
    class X509Sample
    {
        // Enter the Subject Name of your certiticate here. YOu can obtain your certificate here:
        // http://aws-portal.amazon.com/gp/aws/developer/account/index.html?action=access-key
        // 
        // See the accompanying README.html on how to make the downloaded certificate available
        // to this sample
        const String MY_AWS_CERT_NAME = "CN=xxxxx, OU=AWS-Developers, O=Amazon.com, C=US";

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
        // Note: protocol must be https.
        const String DESTINATION      = "https://ecs.amazonaws.com/onca/soap?Service=AWSECommerceService";

        // Enter the AWS Access Key ID for you account here
        const String MY_AWS_ACCESS_KEY_ID = "YOUR_AWS_ACCESS_KEY_ID_HERE";

        static void Main(string[] args)
        {
            // If you are using a debugging proxy like Fiddler to capture SOAP
            // envelopes, and you get SSL Certificate warnings, uncomment the 
            // following line:
            // ServicePointManager.ServerCertificateValidationCallback = delegate { return true; };

            // create an ItemLookup request:
            ItemLookup itemLookup = new ItemLookup();
            itemLookup.AWSAccessKeyId = MY_AWS_ACCESS_KEY_ID;

            ItemLookupRequest itemLookupRequest = new ItemLookupRequest();
            itemLookupRequest.ItemId = new String[] { "0545010225" };
            itemLookupRequest.ResponseGroup = new String[] { "Small", "AlternateVersions" };
            itemLookup.Request = new ItemLookupRequest[] { itemLookupRequest };

            // create service instance and set destination
            AWSECommerceService api = new AWSECommerceService();
            api.Destination = new Uri(DESTINATION);

            // set the policy so that all outgoing requests are signed with the certificate
            AmazonX509Assertion amazonX509Assertion = new AmazonX509Assertion(MY_AWS_CERT_NAME);
            api.SetPolicy(amazonX509Assertion.Policy());

            // fetch the response.
            ItemLookupResponse itemLookupResponse = api.ItemLookup(itemLookup);

            Item item = null;
            try {
                item = itemLookupResponse.Items[0].Item[0];
                System.Console.WriteLine(item.ItemAttributes.Title);
            } catch (Exception e) {
                System.Console.Error.WriteLine(e);
            }

            System.Console.WriteLine("X.509 sample finished. Hit Enter to terminate.");
            System.Console.ReadLine();
        }
    }
}
