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

package com.amazon.advertising.api.sample;

import java.net.MalformedURLException;
import java.net.URL;
import java.rmi.RemoteException;

import javax.xml.rpc.ServiceException;

import org.apache.axis.ConfigurationException;
import org.apache.axis.EngineConfiguration;
import org.apache.axis.configuration.FileProvider;

import com.amazon.advertising.api.AWSECommerceService;
import com.amazon.advertising.api.AWSECommerceServiceLocator;
import com.amazon.advertising.api.AWSECommerceServicePortType;
import com.amazon.advertising.api.Item;
import com.amazon.advertising.api.ItemLookup;
import com.amazon.advertising.api.ItemLookupRequest;
import com.amazon.advertising.api.ItemLookupResponse;



public class SampleItemLookup {

    /*
     * Use one of the following end-points, according to the region you are interested in.
     * 
     * 	US:	soap.amazon.com
     * 	CA:	soap.amazon.ca
     * 	UK:	soap.amazon.co.uk
     * 	DE:	soap.amazon.de
     * 	FR:	soap.amazon.fr
     * 	JP:	soap.amazon.co.jp
     * 
     * If you want to use tcpmon[1] to capture the outgoing and incoming
     * SOAP envelopes, set it up to listen on port 8080 and
     * forward to one of the end-points above and set END_POINT below
     * to "localhost:8080"
     * 
     * [1] https://tcpmon.dev.java.net/
     * 
     */
    private static final String END_POINT = "soap.amazon.com";

    /*
     * There are two ways to authenticate your requests:
     *   (1) WS-Security X.509 Token Profile: http://www.oasis-open.org/committees/download.php/16785/wss-v1.1-spec-os-x509TokenProfile.pdf
     *   (2) Amazon's Secret-Key-HMAC authentication.
     *   
     * Depending on which one you want to use, set the value of the constant below to either
     *	 (1) "x509.wsdd", or
     *	 (2) "hmac.wsdd"
     *
     * Check the README for setting up either choice.
     */
    private static final String WSDD = "x509.wsdd";

    /*
     * The AWS Access Key ID that corresponds to the identity you wish to use.
     */
    private static final String AWS_ACCESS_KEY_ID = "YOUR_AWS_ACCESS_KEY_ID_HERE";
    
    private static URL PORT_ADDRESS;
    static {
        try {
            PORT_ADDRESS = new URL("https://" + END_POINT + "/onca/soap?Service=AWSECommerceService");
        } catch (MalformedURLException e) {
            e.printStackTrace();
        }
    }

    /*
     * Which ASIN to lookup up.
     */
    private static final String MY_ITEM_ID = "0545010225";

    public static void main(String[] args) {
        try {
            // AxisProperties.setProperty("axis.socketSecureFactory","org.apache.axis.components.net.SunFakeTrustSocketFactory");
            
            // Load the deployment descriptor and use that to obtain an instance of the port.
            // The deployment descriptor tells Axis how to authenticate outgoing request.
            EngineConfiguration serviceConfig = new FileProvider("resources", WSDD);
            AWSECommerceService locator = new AWSECommerceServiceLocator(serviceConfig);
            AWSECommerceServicePortType service = locator.getAWSECommerceServicePort(PORT_ADDRESS);

            // Construct a simple ItemLookup request
            ItemLookupRequest request = new ItemLookupRequest();
            request.setItemId(new String[] { MY_ITEM_ID });
            request.setResponseGroup(new String[] { "Small" });

            ItemLookup lookup = new ItemLookup();
            lookup.setAWSAccessKeyId(AWS_ACCESS_KEY_ID);
            lookup.setRequest(new ItemLookupRequest[] {request});

            // Fetch the response and print out the title
            ItemLookupResponse response = service.itemLookup(lookup);
            Item item = response.getItems(0).getItem(0);
            String title = item.getItemAttributes().getTitle();
            System.out.println("Item " + MY_ITEM_ID + " is titled '" + title + "'");

            // done!
        } catch (ServiceException e) {
            e.printStackTrace();
        } catch (RemoteException e) {
            e.printStackTrace();
        } catch (NullPointerException e) {
            e.printStackTrace();
        } catch (ConfigurationException e) {
            e.printStackTrace();
        }
    }
}
