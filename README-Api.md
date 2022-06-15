# API Documentation
## Quarters Initialisation
Before making any of the Quarters SDK calls you must call the following.

```
private void Start() {
    // Runs the Init function of the Quarters class. Allows further Quarters functionality if completed
    QuartersInit.Instance.Init(OnInitComplete, OnInitError);
}

//Empty function for now. Gets filled in the next step
//Runs after the Init function has successfully finished
private void OnInitComplete() {

}

//Runs if there was an error during the Init process. Returns an error string.
private void OnInitError(string error) {
    Debug.LogError(error);
}
```

## Sign in with Quarters
Once Quarters Init is completed successfully you need to sign in your user

```
private void OnInitComplete() {
    Quarters.Instance.SignInWithQuarters(OnSignInComplete, OnSignInError);
}

private void OnSignInComplete() {

}

private void OnSignInError(string signInError) {
    Debug.Log(signInError);
}
```

## Authorization Screen
When the  SignInWithQuarters function is called, the player will be taken to the Quarters web page on their default browser. 
 
After the player has signed into their Quarters account (if they have not already done so) they will be prompted to allow the game access to the player’s PoQ account information. 


The player must click the **Authorize** button to allow Quarters transactions. Any attempt to exchange Quarters, without being authorized first, will result in an error during the transaction process.

## Quarters script functions
Some comments in the quarters script that may help clarify things for developers
```
//Around line 135 in the script
//May need more info on why it's called deauthorize. Does this function have more to do with deauthorization or signing the user out?
        public void Deauthorize() {
            Session.Invalidate();
            session = null;
            CurrentUser = null;

            Log("Quarters user signed out");
            OnSignOut?.Invoke();
        }
        
//May need more info on what details that are being grabbed
        public void GetUserDetails(Action<User> OnSuccessDelegate, Action<string> OnFailedDelegate) {
            StartCoroutine(GetUserDetailsCall(OnSuccessDelegate, OnFailedDelegate));
        }

//May need more clarification on what inormation about the user's balance is being obtained (the user's quarters, etc.).
        public void GetAccountBalanceCall(Action<long> OnSuccess, Action<string> OnError) {
            StartCoroutine(GetAccountBalance(OnSuccess, OnError));
        }
```
