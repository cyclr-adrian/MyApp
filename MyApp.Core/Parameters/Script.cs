using System;

namespace MyApp.Core.Parameters
{
    /// <summary>
    /// Parameters for the script engine.
    /// </summary>
    public static class Script
    {
        /// <summary>
        /// Time limit for running the script.
        /// </summary>
        public static readonly TimeSpan Timeout = TimeSpan.FromSeconds(90);

        /// <summary>
        /// Recursion depth limit: 100.
        /// </summary>
        public const int RecursionDepthLimit = 100;

        /// <summary>
        /// Memory allocation limit: 2.5GB.
        /// (2.5GB allows us to download a 20MB CSV file.)
        /// </summary>
        public const long MemoryLimit = 2_684_354_560L;


        #region Avaliable Objects
        /// <summary>
        /// The endpoint for the method being called in MyApp.
        /// </summary>
        public const string MethodEndpoint = "method_endpoint";

        /// <summary>
        /// The HTTP method being called in MyApp.
        /// </summary>
        public const string HttpMethod = "http_method";

        /// <summary>
        /// The auth value for the method being called in MyApp.
        /// </summary>
        public const string MethodAuthValue = "method_auth_value";

        /// <summary>
        /// The request message for the method to be called in MyApp.
        /// </summary>
        public const string MethodRequest = "method_request";

        /// <summary>
        /// The raw request from calling a method.
        /// </summary>
        public const string MethodRawRequest = "method_raw_request";

        /// <summary>
        /// The raw response from calling a method.
        /// </summary>
        public const string MethodRawResponse = "method_raw_response";

        /// <summary>
        /// The request mergefields for the method to be called in MyApp.
        /// </summary>
        public const string MethodRequestMergefields = "method_request_mergefields";

        /// <summary>
        /// The request headers for the method to be called in MyApp.
        /// </summary>
        public const string MethodRequestHeaders = "method_request_headers";

        /// <summary>
        /// The request parameters for the method to be called in MyApp.
        /// </summary>
        public const string MethodRequestParameters = "method_request_parameters";

        /// <summary>
        /// The available response field mappings for the method to be called in MyApp.
        /// </summary>
        public const string MethodResponseFields = "method_response_fields";

        /// <summary>
        /// The methods response fields mapped in subsequent steps.
        /// </summary>
        public const string MethodResponseFieldsInUse = "method_response_fields_in_use";

        /// <summary>
        /// The response message for the method being called in MyApp.
        /// </summary>
        public const string MethodResponse = "method_response";

        /// <summary>
        /// The response headers for the method being called in MyApp.
        /// </summary>
        public const string MethodResponseHeaders = "method_response_headers";

        /// <summary>
        /// The error response for a method being called in MyApp.
        /// </summary>
        public const string MethodError = "method_error";

        /// <summary>
        /// Any account connector properties with parameter target type of script.
        /// </summary>
        public const string ScriptParameters = "script_parameters";

        /// <summary>
        /// The last successful run date for the step.
        /// </summary>
        public const string LastSuccessfulRunDate = "last_successful_run_date";

        /// <summary>
        /// The next last successful run date to save in the cycle.
        /// </summary>
        public const string NextLastSuccessfulRunDate = "next_last_successful_run_date";

        /// <summary>
        /// The partner domain.
        /// </summary>
        public const string ServiceDomain = "service_domain";

        /// <summary>
        /// The cycle parameters.
        /// </summary>
        public const string CycleVariables = "cycle_variables";

        /// <summary>
        /// ID of the cycle.
        /// </summary>
        public const string CycleId = "cycle_id";

        /// <summary>
        /// ID of the cycle step.
        /// </summary>
        public const string CycleStepId = "cycle_step_id";

        /// <summary>
        /// Internal Account ID of the account the cycle is installed in.
        /// </summary>
        public const string AccountId = "MyApp_account_id";

        /// <summary>
        /// Transaction ID of the executing transaction.
        /// </summary>
        public const string TransactionId = "MyApp_transaction_id";

        /// <summary>
        /// External Account ID of the account the cycle is installed in.
        /// </summary>
        public const string ExternalAccountId = "external_account_id";

        /// <summary>
        /// The Client ID for an OAuth connector.
        /// </summary>
        public const string OAuthClientId = "oauth_client_id";

        /// <summary>
        /// The Client Secret for an OAuth connector.
        /// </summary>
        public const string OAuthClientSecret = "oauth_client_secret";

        /// <summary>
        /// Temp data store that persists across before_action, after_action and after_action_paging.
        /// </summary>
        public const string ActionData = "action_data";

        /// <summary>
        /// The context the script is being executed in, for example 'running transaction', 'testing step, 'testing connector'
        /// </summary>
        public const string ScriptExecutionContext = "script_execution_context";

        #endregion

        #region Available Functions
        /// <summary>
        /// Make outbound HTTP requests.
        /// </summary>
        public const string HttpRequest = "http_request";

        /// <summary>
        /// Encrypt the input data.
        /// </summary>
        public const string MyAppEncrypt = "MyApp_encrypt";

        /// <summary>
        /// Sign the input data.
        /// </summary>
        public const string MyAppSign = "MyApp_sign";

        /// <summary>
        /// Serialise the input object to XML.
        /// </summary>
        public const string MyAppXmlSerialize = "MyApp_xml_serialize";

        /// <summary>
        /// Deserialise XML to an object.
        /// </summary>
        public const string MyAppXmlDeserialize = "MyApp_xml_deserialize";

        /// <summary>
        /// Parse CSV to an object.
        /// </summary>
        public const string MyAppCsvParse = "MyApp_csv_parse";
        #endregion

        #region Available Events
        /// <summary>
        /// This is called before MyApp catches a webhook.
        /// </summary>
        public const string BeforeWebhook = "before_webhook";

        /// <summary>
        /// This is called after MyApp catches a webhook.
        /// </summary>
        public const string AfterWebhook = "after_webhook";

        /// <summary>
        /// This is called before MyApp starts an action.
        /// </summary>
        public const string BeforeAction = "before_action";

        /// <summary>
        /// This is called before MyApp catches a partner webhook.
        /// </summary>
        public const string BeforePartnerWebhook = "before_partner_webhook";

        /// <summary>
        /// This is called after MyApp finishes an action page.
        /// </summary>
        public const string AfterAction = "after_action";

        /// <summary>
        /// This is called after MyApp finishes all action pages.
        /// </summary>
        public const string AfterActionPaging = "after_action_paging";

        /// <summary>
        /// This is called if MyApp gets an error from an action.
        /// </summary>
        public const string AfterError = "after_error";

        /// <summary>
        /// This is called for action steps that have true/false outputs.
        /// </summary>
        public const string ActionCondition = "action_condition";

        /// <summary>
        /// This is called before MyApp makes the OAuth2 authorise request.
        /// </summary>
        public const string BeforeOAuth2Authorise = "before_oauth2_authorise";

        /// <summary>
        /// This is called before MyApp makes the OAuth2 access token request.
        /// </summary>
        public const string BeforeOAuth2Token = "before_oauth2_token";

        /// <summary>
        /// This is called after MyApp makes the OAuth2 access token request.
        /// </summary>
        public const string AfterOAuth2Token = "after_oauth2_token";

        /// <summary>
        /// This is called before MyApp makes the OAuth2 refresh token request.
        /// </summary>
        public const string BeforeOAuth2Refresh = "before_oauth2_refresh";

        /// <summary>
        /// This is called after MyApp makes the OAuth2 refresh token request.
        /// </summary>
        public const string AfterOAuth2Refresh = "after_oauth2_refresh";

        /// <summary>
        /// This is called when the OAuth2 refresh errors.
        /// </summary>
        public const string AfterOAuth2RefreshError = "after_oauth2_refresh_error";

        /// <summary>
        /// This is called for webhook steps that have true/false outputs.
        /// </summary>
        public const string WebhookCondition = "webhook_condition";

        /// <summary>
        /// This is called before a CycleStep is deleted.
        /// </summary>
        public const string BeforeCycleStepDelete = "before_step_delete";
        #endregion
    }
}