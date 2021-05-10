//using System;
//using System.Threading.Tasks;
//using Windows.ApplicationModel.Store;

//namespace Helpers.Monetization
//{
//    public class InAppPurchaseHelper
//    {
//        private LicenseInformation licenseInformation;

//        public InAppPurchaseHelper()
//        {
//            Init();
//        }

//        private void Init()
//        {
//            // Get the license info
//#if DEBUG
//            // The next line is commented out for production/release.       
//            licenseInformation = CurrentAppSimulator.LicenseInformation;
//#else
//            // The next line is commented out for testing.
//            licenseInformation = CurrentApp.LicenseInformation;
//#endif
//        }

//        public async Task<bool> BuyFeature(string name)
//        {
//            if (!licenseInformation.ProductLicenses[name].IsActive)
//            {
//                try
//                {
//                    // The customer doesn't own this feature, so 
//                    // show the purchase dialog.
//#if DEBUG
//                    var result = await CurrentAppSimulator.RequestProductPurchaseAsync(name);
//#else
//                    var result = await CurrentApp.RequestProductPurchaseAsync(name);
//#endif
//                    //Check the license state to determine if the in-app purchase was successful.
//                    if (result.Status == ProductPurchaseStatus.Succeeded)
//                        return true;
//                    else
//                        return false;
//                }
//                catch (Exception)
//                {
//                    // The in-app purchase was not completed because 
//                    // an error occurred.
//                    return false;
//                }
//            }
//            else
//            {
//                // The customer already owns this feature.
//                return false;
//            }
//        }

//        public bool IsFeatureEnabled(string name)
//        {
//            if (licenseInformation.ProductLicenses[name].IsActive)
//            {
//                // the customer can access this feature
//                return true;
//            }
//            else
//            {
//                // the customer can't access this feature
//                return false;
//            }
//        }
//    }
//}
