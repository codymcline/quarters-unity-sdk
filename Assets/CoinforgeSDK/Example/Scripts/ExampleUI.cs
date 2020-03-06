﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Newtonsoft.Json;
using CoinforgeSDK;
#if QUARTERS_MODULE_IAP
using UnityEngine.Purchasing;
#endif


public class ExampleUI : MonoBehaviour {

    public List<CanvasGroup> authorizedOnlyUI = new List<CanvasGroup>();
    public List<CanvasGroup> unAuthorizedOnlyUI = new List<CanvasGroup>();
    public List<CanvasGroup> playfabModuleOnlyUI = new List<CanvasGroup>();
    public List<CanvasGroup> authorizedGuestOnlyUI = new List<CanvasGroup>();
    public Text debugConsole;

    public InputField tokensInput;
    public InputField descriptionInput;


	void Start() {

        debugConsole.text = "Coinforge SDK example";
        debugConsole.text += "\nUnauthorized";

        RefreshUI();
	}





	private void RefreshUI() {
		authorizedOnlyUI.ForEach(b => b.interactable = Coinforge.Instance.IsAuthorized);
        unAuthorizedOnlyUI.ForEach(b => b.interactable = !Coinforge.Instance.IsAuthorized);


        if (Coinforge.Instance.IsAuthorized) {
            authorizedOnlyUI.ForEach(b => b.alpha = 1f);
            unAuthorizedOnlyUI.ForEach(b => b.alpha = 0.4f);
        }
        else {
            authorizedOnlyUI.ForEach(b => b.alpha = 0.4f);
            unAuthorizedOnlyUI.ForEach(b => b.alpha = 1f);
        }

     
        //guest mode only
        authorizedGuestOnlyUI.ForEach(b => b.interactable = Coinforge.Instance.IsAuthorized && Coinforge.Instance.session.IsGuestSession);
        if (Coinforge.Instance.IsAuthorized && Coinforge.Instance.session.IsGuestSession) {
            authorizedGuestOnlyUI.ForEach(b => b.alpha = 1f);
        }
        else {
            authorizedGuestOnlyUI.ForEach(b => b.alpha = 0.4f);
        }

	}



    public void ButtonAuthorizeGuestTapped() {
        Coinforge.Instance.AuthorizeGuest(OnAuthorizationSuccess, OnAuthorizationFailed);

    }

	public void ButtonAuthorizeTapped() {
		Coinforge.Instance.Authorize(OnAuthorizationSuccess, OnAuthorizationFailed);

	}

    public void ButtonDeauthorizeTapped() {
        Coinforge.Instance.Deauthorize();
        RefreshUI();
    }


    public void ButtonSigupTapped() {
        Coinforge.Instance.SignUp(OnAuthorizationSuccess, OnAuthorizationFailed);
    }



	public void ButtonGetUserDetailsTapped() {
		Coinforge.Instance.GetUserDetails(delegate(User user) {
			Debug.Log("User loaded");

            debugConsole.text += "\n";
            debugConsole.text += "\nUser loaded: ";
            debugConsole.text += JsonConvert.SerializeObject(user, Formatting.Indented);

		}, delegate (string error) {
			Debug.LogError("Cannot load the user details: " + error);
            debugConsole.text += "\n";
            debugConsole.text += "\nCannot load the user details:: " + error;
		});
	}





	public void OnAuthorizationSuccess() {
		Debug.Log("OnAuthorizationSuccess");

        debugConsole.text += "\n";
        debugConsole.text += "\nOnAuthorizationSuccess";

		RefreshUI();
	}


	public void OnAuthorizationFailed(string error) {
		Debug.LogError("OnAuthorizationFailed: " + error);

        debugConsole.text += "\n";
        debugConsole.text += "\nOnAuthorizationFailed: " + error;

		RefreshUI();
	}




    public void ButtonGetAccountsTapped() {
        
        Coinforge.Instance.GetAccounts(delegate (List<User.Account> accounts) {

            debugConsole.text += "\n";
            debugConsole.text += "\nOnGetAccountsSuccess";
            debugConsole.text += JsonConvert.SerializeObject(accounts, Formatting.Indented);

            RefreshUI();
            
        }, delegate (string error) {

            debugConsole.text += "\n";
            debugConsole.text += "\nOnGetAccountsFailed: " + error;

            RefreshUI();

        });
    }




    public void ButtonGetFirstAccountBalanceTapped() {

        Coinforge.Instance.GetAccountBalance(delegate (User.Account.Balance balance) {

            debugConsole.text += "\n";
            debugConsole.text += "\nOnGetAccountBalanceSuccess";
            debugConsole.text += JsonConvert.SerializeObject(balance, Formatting.Indented);

            RefreshUI();

        }, delegate (string error) {

            debugConsole.text += "\n";
            debugConsole.text += "\nOnGetAccountBalanceFailed: " + error;

            RefreshUI();

        });
    }



    public void ButtonGetAccountRewardTapped() {

        Coinforge.Instance.GetAccountReward(delegate (User.Account.Reward balance) {

            debugConsole.text += "\n";
            debugConsole.text += "\nOnGetAccountRewardSuccess";
            debugConsole.text += JsonConvert.SerializeObject(balance, Formatting.Indented);

            RefreshUI();

        }, delegate (string error) {

            debugConsole.text += "\n";
            debugConsole.text += "\nOnGetAccountRewardFailed: " + error;

            RefreshUI();

        });
    }








    public void ButtonAwardQuartersTapped() {
        
            
        //Request 2 quarters from Playfab Cloud build
        Coinforge.Instance.Award(10, delegate(string transactionHash) {

            Debug.Log("Coinforge awarded: " + transactionHash);

        }, delegate (string error) {

            debugConsole.text += "\n";
            debugConsole.text += "\nOnAwardQuartersFailed: " + error;

            RefreshUI();

        });
 



    }





    public void ButtonInitializeTapped() {

        #if QUARTERS_MODULE_IAP

        List<string> testProducts = new List<string>();
        testProducts.Add(Application.identifier + ".100" + Constants.QUARTERS_PRODUCT_KEY);

        CoinforgeIAP.Instance.Initialize(testProducts, delegate(Product[] products) {

        }, delegate(InitializationFailureReason reason) {
        
            Debug.LogError(reason.ToString());
        });


        #else
        Debug.LogError("Coinforge module: IAP, is not enabled. Add Coinforge IAP module");
        #endif

    }
     




    public void ButtonBuyIAPTapped() {

        #if QUARTERS_MODULE_IAP

        if (Application.isEditor) {
            Debug.LogError("Buying IAP is not supported in Unity Editor, please test on iOS or Android device");
            return;
        }


        if (CoinforgeIAP.Instance.products.Count == 0) {
            Debug.LogError("No products loaded. Call CoinforgeIAP.Initialize first!");
            return;
        }


        Coinforge.Instance.GetUserDetails(delegate(User user) {
            Debug.Log("User loaded");

            debugConsole.text += "\n";
            debugConsole.text += "\nUser loaded: ";
            debugConsole.text += JsonConvert.SerializeObject(user, Formatting.Indented);

            //test purchase of first initialized product
            CoinforgeIAP.Instance.BuyProduct(CoinforgeIAP.Instance.products[0], (Product product, string txId) => {

                Debug.Log("Purchase complete");
                debugConsole.text += "\n";
                debugConsole.text += "\nTransfer successful, transactionHash: " + txId;
                Debug.Log("Console: " + debugConsole.text);


            },(string error) => {
                Debug.LogError("Purchase error: " + error);

                debugConsole.text += "\n";
                debugConsole.text += "\nOnTransactionFailed: " + error;
                Debug.Log("Console: " + debugConsole.text);
            });


        }, delegate (string error) {
            Debug.LogError("Cannot load the user details: " + error);
            debugConsole.text += "\n";
            debugConsole.text += "\nCannot load the user details:: " + error;
        });

        #else
        Debug.LogError("Coinforge module: IAP, is not enabled. Add Coinforge IAP module");
        #endif

    }




    public void ButtonTransferTapped() {
        
        Debug.Log("ButtonTransferTapped");

        TransferAPIRequest request = new TransferAPIRequest(int.Parse(tokensInput.text), descriptionInput.text, delegate (string transactionHash) {
        
            debugConsole.text += "\n";
            debugConsole.text += "\nTransfer successful, transactionHash: " + transactionHash;

        }, delegate (string error) {
        
            debugConsole.text += "\n";
            debugConsole.text += "\nOnTransactionFailed: " + error;
            Debug.LogError(error);
        });



        Coinforge.Instance.CreateTransfer(request);


    }





}

