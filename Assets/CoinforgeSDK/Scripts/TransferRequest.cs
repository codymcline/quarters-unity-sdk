﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Newtonsoft.Json;
using UnityEngine.Assertions;

namespace CoinforgeSDK {
    public class TransferRequest {

        public string id = "";
        public int tokens = 0;
        public string appId = "";
        public string appAddress = "";
        public string embedCode = "";
        public string description = "";
        public string type = "";
        public string endpoint = "";
        public string userId = "";
        public object created = null;


        public TransferRequest () {
            
        }


        public TransferRequest (string json) {
            TransferRequest result = JsonConvert.DeserializeObject<TransferRequest>(json);
            this.id = result.id;
            this.tokens = result.tokens;
            this.appId = result.appId;
            this.appAddress = result.appAddress;
            this.embedCode = result.embedCode;
            this.description = result.description;
            this.type = result.type;
            this.endpoint = result.endpoint;
            this.created = result.created;
            this.userId = result.userId;
        }

    }


    public class TransferAPIRequest {
        public string requestId = "";
        public int tokens = 0;
        public string description = "";
        public string txId = "";
        public string firebaseToken = "";

        public Coinforge.OnTransferSuccessDelegate successDelegate;
        public Coinforge.OnTransferFailedDelegate failedDelegate;

        public TransferAPIRequest (int tokens, Coinforge.OnTransferSuccessDelegate successDelegate, Coinforge.OnTransferFailedDelegate failedDelegate) {
            
            Assert.IsTrue(tokens > 0, "Transfer tokens must be larger than zero");
            
            this.tokens = tokens;
            this.successDelegate = successDelegate;
            this.failedDelegate = failedDelegate;
        }

        public TransferAPIRequest(int tokens, string description, Coinforge.OnTransferSuccessDelegate successDelegate, Coinforge.OnTransferFailedDelegate failedDelegate) {
            
            Assert.IsTrue(tokens > 0, "Transfer tokens must be larger than zero");
            
            this.tokens = tokens;
            this.description = description;
            this.successDelegate = successDelegate;
            this.failedDelegate = failedDelegate;
        }




    }
}