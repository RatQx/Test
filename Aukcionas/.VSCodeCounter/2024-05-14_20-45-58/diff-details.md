# Diff Details

Date : 2024-05-14 20:45:58

Directory c:\\Users\\Tautvydas\\Desktop\\Auction-master\\Auction-master\\Aukcionas\\XUnitTests

Total : 283 files,  35328 codes, -197 comments, -5422 blanks, all 29709 lines

[Summary](results.md) / [Details](details.md) / [Diff Summary](diff.md) / Diff Details

## Files
| filename | language | code | comment | blank | total |
| :--- | :--- | ---: | ---: | ---: | ---: |
| [Aukcionas/.embold/vscode-logs.log](/Aukcionas/.embold/vscode-logs.log) | Log | -14 | 0 | -2 | -16 |
| [Aukcionas/Aukcionas.csproj](/Aukcionas/Aukcionas.csproj) | MSBuild | -31 | 0 | -4 | -35 |
| [Aukcionas/Aukcionas.http](/Aukcionas/Aukcionas.http) | HTTP | -3 | -1 | -3 | -7 |
| [Aukcionas/Aukcionas.sln](/Aukcionas/Aukcionas.sln) | Solution File | -24 | 0 | -2 | -26 |
| [Aukcionas/Auth/JwtTokenService.cs](/Aukcionas/Auth/JwtTokenService.cs) | C# | -42 | -1 | -4 | -47 |
| [Aukcionas/Auth/Model/AuthDtos.cs](/Aukcionas/Auth/Model/AuthDtos.cs) | C# | -8 | 0 | -5 | -13 |
| [Aukcionas/Auth/Model/ForumRestUser.cs](/Aukcionas/Auth/Model/ForumRestUser.cs) | C# | -40 | 0 | -3 | -43 |
| [Aukcionas/Auth/Model/ForumRoles.cs](/Aukcionas/Auth/Model/ForumRoles.cs) | C# | -9 | 0 | -2 | -11 |
| [Aukcionas/Auth/Model/IUserOwnedResource.cs](/Aukcionas/Auth/Model/IUserOwnedResource.cs) | C# | -7 | 0 | -1 | -8 |
| [Aukcionas/Auth/Model/PolicyNames.cs](/Aukcionas/Auth/Model/PolicyNames.cs) | C# | -7 | 0 | -1 | -8 |
| [Aukcionas/Auth/Model/ResetPasswordDto.cs](/Aukcionas/Auth/Model/ResetPasswordDto.cs) | C# | -10 | 0 | -1 | -11 |
| [Aukcionas/Auth/ResourceOwnerAuthorizationHandler.cs](/Aukcionas/Auth/ResourceOwnerAuthorizationHandler.cs) | C# | -19 | 0 | -2 | -21 |
| [Aukcionas/Controllers/AdminController.cs](/Aukcionas/Controllers/AdminController.cs) | C# | -133 | 0 | -10 | -143 |
| [Aukcionas/Controllers/AuctionController.cs](/Aukcionas/Controllers/AuctionController.cs) | C# | -482 | -3 | -48 | -533 |
| [Aukcionas/Controllers/AuthController.cs](/Aukcionas/Controllers/AuthController.cs) | C# | -337 | -2 | -28 | -367 |
| [Aukcionas/Controllers/ChatController.cs](/Aukcionas/Controllers/ChatController.cs) | C# | -158 | 0 | -16 | -174 |
| [Aukcionas/Controllers/LivestreamController.cs](/Aukcionas/Controllers/LivestreamController.cs) | C# | -77 | -1 | -8 | -86 |
| [Aukcionas/Controllers/PaymentController.cs](/Aukcionas/Controllers/PaymentController.cs) | C# | -274 | 0 | -22 | -296 |
| [Aukcionas/Controllers/ReportController.cs](/Aukcionas/Controllers/ReportController.cs) | C# | -152 | 0 | -18 | -170 |
| [Aukcionas/Data/AuthDbSeeder.cs](/Aukcionas/Data/AuthDbSeeder.cs) | C# | -46 | 0 | -7 | -53 |
| [Aukcionas/Data/DataContext.cs](/Aukcionas/Data/DataContext.cs) | C# | -19 | 0 | -4 | -23 |
| [Aukcionas/Data/dbtrigger.sql](/Aukcionas/Data/dbtrigger.sql) | SQL | -29 | -22 | -6 | -57 |
| [Aukcionas/Migrations/20240303113743_UserInfo.Designer.cs](/Aukcionas/Migrations/20240303113743_UserInfo.Designer.cs) | C# | -304 | -2 | -107 | -413 |
| [Aukcionas/Migrations/20240303113743_UserInfo.cs](/Aukcionas/Migrations/20240303113743_UserInfo.cs) | C# | -245 | -3 | -25 | -273 |
| [Aukcionas/Migrations/20240303114439_Init.Designer.cs](/Aukcionas/Migrations/20240303114439_Init.Designer.cs) | C# | -304 | -2 | -107 | -413 |
| [Aukcionas/Migrations/20240303114439_Init.cs](/Aukcionas/Migrations/20240303114439_Init.cs) | C# | -14 | -3 | -6 | -23 |
| [Aukcionas/Migrations/20240303170547_PasswordReset.Designer.cs](/Aukcionas/Migrations/20240303170547_PasswordReset.Designer.cs) | C# | -308 | -2 | -109 | -419 |
| [Aukcionas/Migrations/20240303170547_PasswordReset.cs](/Aukcionas/Migrations/20240303170547_PasswordReset.cs) | C# | -32 | -3 | -6 | -41 |
| [Aukcionas/Migrations/20240309104956_EmailConfirm.Designer.cs](/Aukcionas/Migrations/20240309104956_EmailConfirm.Designer.cs) | C# | -313 | -2 | -111 | -426 |
| [Aukcionas/Migrations/20240309104956_EmailConfirm.cs](/Aukcionas/Migrations/20240309104956_EmailConfirm.cs) | C# | -32 | -3 | -6 | -41 |
| [Aukcionas/Migrations/20240309110353_EmailConfirmfix.Designer.cs](/Aukcionas/Migrations/20240309110353_EmailConfirmfix.Designer.cs) | C# | -313 | -2 | -111 | -426 |
| [Aukcionas/Migrations/20240309110353_EmailConfirmfix.cs](/Aukcionas/Migrations/20240309110353_EmailConfirmfix.cs) | C# | -31 | -3 | -4 | -38 |
| [Aukcionas/Migrations/20240309110724_EmailConfirmfixv2.Designer.cs](/Aukcionas/Migrations/20240309110724_EmailConfirmfixv2.Designer.cs) | C# | -312 | -2 | -111 | -425 |
| [Aukcionas/Migrations/20240309110724_EmailConfirmfixv2.cs](/Aukcionas/Migrations/20240309110724_EmailConfirmfixv2.cs) | C# | -30 | -3 | -4 | -37 |
| [Aukcionas/Migrations/20240309141517_auctionLikes.Designer.cs](/Aukcionas/Migrations/20240309141517_auctionLikes.Designer.cs) | C# | -315 | -2 | -112 | -429 |
| [Aukcionas/Migrations/20240309141517_auctionLikes.cs](/Aukcionas/Migrations/20240309141517_auctionLikes.cs) | C# | -22 | -3 | -4 | -29 |
| [Aukcionas/Migrations/20240309164102_auctionComments.Designer.cs](/Aukcionas/Migrations/20240309164102_auctionComments.Designer.cs) | C# | -350 | -2 | -124 | -476 |
| [Aukcionas/Migrations/20240309164102_auctionComments.cs](/Aukcionas/Migrations/20240309164102_auctionComments.cs) | C# | -42 | -3 | -5 | -50 |
| [Aukcionas/Migrations/20240309182852_auctionCommentsv2.Designer.cs](/Aukcionas/Migrations/20240309182852_auctionCommentsv2.Designer.cs) | C# | -350 | -2 | -124 | -476 |
| [Aukcionas/Migrations/20240309182852_auctionCommentsv2.cs](/Aukcionas/Migrations/20240309182852_auctionCommentsv2.cs) | C# | -62 | -3 | -14 | -79 |
| [Aukcionas/Migrations/20240309185752_auctionCommentsv3.Designer.cs](/Aukcionas/Migrations/20240309185752_auctionCommentsv3.Designer.cs) | C# | -350 | -2 | -124 | -476 |
| [Aukcionas/Migrations/20240309185752_auctionCommentsv3.cs](/Aukcionas/Migrations/20240309185752_auctionCommentsv3.cs) | C# | -14 | -3 | -6 | -23 |
| [Aukcionas/Migrations/20240309190226_auctionCommentsv4.Designer.cs](/Aukcionas/Migrations/20240309190226_auctionCommentsv4.Designer.cs) | C# | -350 | -2 | -124 | -476 |
| [Aukcionas/Migrations/20240309190226_auctionCommentsv4.cs](/Aukcionas/Migrations/20240309190226_auctionCommentsv4.cs) | C# | -14 | -3 | -6 | -23 |
| [Aukcionas/Migrations/20240309193035_auctionCommentsv5.Designer.cs](/Aukcionas/Migrations/20240309193035_auctionCommentsv5.Designer.cs) | C# | -350 | -2 | -124 | -476 |
| [Aukcionas/Migrations/20240309193035_auctionCommentsv5.cs](/Aukcionas/Migrations/20240309193035_auctionCommentsv5.cs) | C# | -14 | -3 | -6 | -23 |
| [Aukcionas/Migrations/20240314092352_Report.Designer.cs](/Aukcionas/Migrations/20240314092352_Report.Designer.cs) | C# | -369 | -2 | -132 | -503 |
| [Aukcionas/Migrations/20240314092352_Report.cs](/Aukcionas/Migrations/20240314092352_Report.cs) | C# | -32 | -3 | -4 | -39 |
| [Aukcionas/Migrations/20240320203933_auctionPictures.Designer.cs](/Aukcionas/Migrations/20240320203933_auctionPictures.Designer.cs) | C# | -372 | -2 | -133 | -507 |
| [Aukcionas/Migrations/20240320203933_auctionPictures.cs](/Aukcionas/Migrations/20240320203933_auctionPictures.cs) | C# | -23 | -3 | -4 | -30 |
| [Aukcionas/Migrations/20240321090650_FixPRoblem.Designer.cs](/Aukcionas/Migrations/20240321090650_FixPRoblem.Designer.cs) | C# | -369 | -2 | -132 | -503 |
| [Aukcionas/Migrations/20240321090650_FixPRoblem.cs](/Aukcionas/Migrations/20240321090650_FixPRoblem.cs) | C# | -23 | -3 | -4 | -30 |
| [Aukcionas/Migrations/20240321111253_PaypalConfirmation.Designer.cs](/Aukcionas/Migrations/20240321111253_PaypalConfirmation.Designer.cs) | C# | -371 | -2 | -133 | -506 |
| [Aukcionas/Migrations/20240321111253_PaypalConfirmation.cs](/Aukcionas/Migrations/20240321111253_PaypalConfirmation.cs) | C# | -22 | -3 | -4 | -29 |
| [Aukcionas/Migrations/20240322095802_RomovedPaypalEmail.Designer.cs](/Aukcionas/Migrations/20240322095802_RomovedPaypalEmail.Designer.cs) | C# | -369 | -2 | -132 | -503 |
| [Aukcionas/Migrations/20240322095802_RomovedPaypalEmail.cs](/Aukcionas/Migrations/20240322095802_RomovedPaypalEmail.cs) | C# | -22 | -3 | -4 | -29 |
| [Aukcionas/Migrations/20240323144525_Payment.Designer.cs](/Aukcionas/Migrations/20240323144525_Payment.Designer.cs) | C# | -402 | -2 | -145 | -549 |
| [Aukcionas/Migrations/20240323144525_Payment.cs](/Aukcionas/Migrations/20240323144525_Payment.cs) | C# | -44 | -3 | -6 | -53 |
| [Aukcionas/Migrations/20240323145205_UpdatePayment.Designer.cs](/Aukcionas/Migrations/20240323145205_UpdatePayment.Designer.cs) | C# | -404 | -2 | -146 | -552 |
| [Aukcionas/Migrations/20240323145205_UpdatePayment.cs](/Aukcionas/Migrations/20240323145205_UpdatePayment.cs) | C# | -23 | -3 | -4 | -30 |
| [Aukcionas/Migrations/20240323164853_PaymentV3.Designer.cs](/Aukcionas/Migrations/20240323164853_PaymentV3.Designer.cs) | C# | -410 | -2 | -148 | -560 |
| [Aukcionas/Migrations/20240323164853_PaymentV3.cs](/Aukcionas/Migrations/20240323164853_PaymentV3.cs) | C# | -32 | -3 | -6 | -41 |
| [Aukcionas/Migrations/20240323175511_PaymentV4.Designer.cs](/Aukcionas/Migrations/20240323175511_PaymentV4.Designer.cs) | C# | -413 | -2 | -149 | -564 |
| [Aukcionas/Migrations/20240323175511_PaymentV4.cs](/Aukcionas/Migrations/20240323175511_PaymentV4.cs) | C# | -23 | -3 | -4 | -30 |
| [Aukcionas/Migrations/20240326163751_user_payment_info.Designer.cs](/Aukcionas/Migrations/20240326163751_user_payment_info.Designer.cs) | C# | -438 | -2 | -161 | -601 |
| [Aukcionas/Migrations/20240326163751_user_payment_info.cs](/Aukcionas/Migrations/20240326163751_user_payment_info.cs) | C# | -112 | -3 | -26 | -141 |
| [Aukcionas/Migrations/20240402152240_Photos.Designer.cs](/Aukcionas/Migrations/20240402152240_Photos.Designer.cs) | C# | -441 | -2 | -162 | -605 |
| [Aukcionas/Migrations/20240402152240_Photos.cs](/Aukcionas/Migrations/20240402152240_Photos.cs) | C# | -23 | -3 | -4 | -30 |
| [Aukcionas/Migrations/20240416163615_AiModel.Designer.cs](/Aukcionas/Migrations/20240416163615_AiModel.Designer.cs) | C# | -441 | -2 | -162 | -605 |
| [Aukcionas/Migrations/20240416163615_AiModel.cs](/Aukcionas/Migrations/20240416163615_AiModel.cs) | C# | -14 | -3 | -6 | -23 |
| [Aukcionas/Migrations/20240416163842_AiModelv2.Designer.cs](/Aukcionas/Migrations/20240416163842_AiModelv2.Designer.cs) | C# | -456 | -2 | -169 | -627 |
| [Aukcionas/Migrations/20240416163842_AiModelv2.cs](/Aukcionas/Migrations/20240416163842_AiModelv2.cs) | C# | -30 | -3 | -4 | -37 |
| [Aukcionas/Migrations/20240416164500_AiModelv3.Designer.cs](/Aukcionas/Migrations/20240416164500_AiModelv3.Designer.cs) | C# | -457 | -2 | -169 | -628 |
| [Aukcionas/Migrations/20240416164500_AiModelv3.cs](/Aukcionas/Migrations/20240416164500_AiModelv3.cs) | C# | -28 | -3 | -4 | -35 |
| [Aukcionas/Migrations/20240416164744_AiModelv4.Designer.cs](/Aukcionas/Migrations/20240416164744_AiModelv4.Designer.cs) | C# | -459 | -2 | -170 | -631 |
| [Aukcionas/Migrations/20240416164744_AiModelv4.cs](/Aukcionas/Migrations/20240416164744_AiModelv4.cs) | C# | -24 | -3 | -4 | -31 |
| [Aukcionas/Migrations/20240416170216_AiModelv5.Designer.cs](/Aukcionas/Migrations/20240416170216_AiModelv5.Designer.cs) | C# | -459 | -2 | -170 | -631 |
| [Aukcionas/Migrations/20240416170216_AiModelv5.cs](/Aukcionas/Migrations/20240416170216_AiModelv5.cs) | C# | -14 | -3 | -6 | -23 |
| [Aukcionas/Migrations/20240416191817_AiModelv6.Designer.cs](/Aukcionas/Migrations/20240416191817_AiModelv6.Designer.cs) | C# | -474 | -2 | -176 | -652 |
| [Aukcionas/Migrations/20240416191817_AiModelv6.cs](/Aukcionas/Migrations/20240416191817_AiModelv6.cs) | C# | -64 | -3 | -8 | -75 |
| [Aukcionas/Migrations/20240417162726_AiModelv7.Designer.cs](/Aukcionas/Migrations/20240417162726_AiModelv7.Designer.cs) | C# | -474 | -2 | -176 | -652 |
| [Aukcionas/Migrations/20240417162726_AiModelv7.cs](/Aukcionas/Migrations/20240417162726_AiModelv7.cs) | C# | -14 | -3 | -6 | -23 |
| [Aukcionas/Migrations/20240418131531_AiModelv8.Designer.cs](/Aukcionas/Migrations/20240418131531_AiModelv8.Designer.cs) | C# | -477 | -2 | -177 | -656 |
| [Aukcionas/Migrations/20240418131531_AiModelv8.cs](/Aukcionas/Migrations/20240418131531_AiModelv8.cs) | C# | -23 | -3 | -4 | -30 |
| [Aukcionas/Migrations/20240418174717_CollectData.Designer.cs](/Aukcionas/Migrations/20240418174717_CollectData.Designer.cs) | C# | -479 | -2 | -178 | -659 |
| [Aukcionas/Migrations/20240418174717_CollectData.cs](/Aukcionas/Migrations/20240418174717_CollectData.cs) | C# | -22 | -3 | -4 | -29 |
| [Aukcionas/Migrations/20240419092148_ChatFunction.Designer.cs](/Aukcionas/Migrations/20240419092148_ChatFunction.Designer.cs) | C# | -479 | -2 | -178 | -659 |
| [Aukcionas/Migrations/20240419092148_ChatFunction.cs](/Aukcionas/Migrations/20240419092148_ChatFunction.cs) | C# | -14 | -3 | -6 | -23 |
| [Aukcionas/Migrations/20240419093213_ChatFunctionv2.Designer.cs](/Aukcionas/Migrations/20240419093213_ChatFunctionv2.Designer.cs) | C# | -557 | -2 | -209 | -768 |
| [Aukcionas/Migrations/20240419093213_ChatFunctionv2.cs](/Aukcionas/Migrations/20240419093213_ChatFunctionv2.cs) | C# | -92 | -3 | -11 | -106 |
| [Aukcionas/Migrations/20240419173713_ChatFunctionv3.Designer.cs](/Aukcionas/Migrations/20240419173713_ChatFunctionv3.Designer.cs) | C# | -517 | -2 | -194 | -713 |
| [Aukcionas/Migrations/20240419173713_ChatFunctionv3.cs](/Aukcionas/Migrations/20240419173713_ChatFunctionv3.cs) | C# | -84 | -3 | -14 | -101 |
| [Aukcionas/Migrations/20240420100142_ChatFunctionv4.Designer.cs](/Aukcionas/Migrations/20240420100142_ChatFunctionv4.Designer.cs) | C# | -535 | -2 | -203 | -740 |
| [Aukcionas/Migrations/20240420100142_ChatFunctionv4.cs](/Aukcionas/Migrations/20240420100142_ChatFunctionv4.cs) | C# | -112 | -3 | -24 | -139 |
| [Aukcionas/Migrations/20240420111413_Chatfunctionv5.Designer.cs](/Aukcionas/Migrations/20240420111413_Chatfunctionv5.Designer.cs) | C# | -526 | -2 | -197 | -725 |
| [Aukcionas/Migrations/20240420111413_Chatfunctionv5.cs](/Aukcionas/Migrations/20240420111413_Chatfunctionv5.cs) | C# | -50 | -2 | -10 | -62 |
| [Aukcionas/Migrations/DataContextModelSnapshot.cs](/Aukcionas/Migrations/DataContextModelSnapshot.cs) | C# | -524 | -1 | -197 | -722 |
| [Aukcionas/Models/Auction.cs](/Aukcionas/Models/Auction.cs) | C# | -99 | 0 | -4 | -103 |
| [Aukcionas/Models/Chat.cs](/Aukcionas/Models/Chat.cs) | C# | -12 | 0 | -1 | -13 |
| [Aukcionas/Models/DecodedPaymentToken.cs](/Aukcionas/Models/DecodedPaymentToken.cs) | C# | -8 | 0 | -1 | -9 |
| [Aukcionas/Models/EmailModel.cs](/Aukcionas/Models/EmailModel.cs) | C# | -17 | 0 | -1 | -18 |
| [Aukcionas/Models/Message.cs](/Aukcionas/Models/Message.cs) | C# | -13 | 0 | -1 | -14 |
| [Aukcionas/Models/PayPalAccessToken.cs](/Aukcionas/Models/PayPalAccessToken.cs) | C# | -11 | 0 | -1 | -12 |
| [Aukcionas/Models/Payment.cs](/Aukcionas/Models/Payment.cs) | C# | -25 | 0 | -2 | -27 |
| [Aukcionas/Models/Recommendations.cs](/Aukcionas/Models/Recommendations.cs) | C# | -20 | 0 | -2 | -22 |
| [Aukcionas/Models/Report.cs](/Aukcionas/Models/Report.cs) | C# | -14 | 0 | -2 | -16 |
| [Aukcionas/Models/UserRecommendations.cs](/Aukcionas/Models/UserRecommendations.cs) | C# | -9 | 0 | -1 | -10 |
| [Aukcionas/Program.cs](/Aukcionas/Program.cs) | C# | -125 | 0 | -18 | -143 |
| [Aukcionas/Properties/launchSettings.json](/Aukcionas/Properties/launchSettings.json) | JSON | -41 | 0 | -1 | -42 |
| [Aukcionas/Services/AuctionStatusService.cs](/Aukcionas/Services/AuctionStatusService.cs) | C# | -85 | 0 | -12 | -97 |
| [Aukcionas/Services/BiddingHub.cs](/Aukcionas/Services/BiddingHub.cs) | C# | -99 | 0 | -11 | -110 |
| [Aukcionas/Services/CloudStorageService.cs](/Aukcionas/Services/CloudStorageService.cs) | C# | -95 | 0 | -6 | -101 |
| [Aukcionas/Services/DailycoService.cs](/Aukcionas/Services/DailycoService.cs) | C# | -103 | 0 | -5 | -108 |
| [Aukcionas/Services/EmailService.cs](/Aukcionas/Services/EmailService.cs) | C# | -45 | 0 | -3 | -48 |
| [Aukcionas/Services/RecommendationService.cs](/Aukcionas/Services/RecommendationService.cs) | C# | -27 | 0 | -5 | -32 |
| [Aukcionas/Utils/AuctionRecommender.cs](/Aukcionas/Utils/AuctionRecommender.cs) | C# | -132 | 0 | -32 | -164 |
| [Aukcionas/Utils/AuctionStatusChecker.cs](/Aukcionas/Utils/AuctionStatusChecker.cs) | C# | -32 | 0 | -6 | -38 |
| [Aukcionas/Utils/ConfigOptions/GCSConfigOptions.cs](/Aukcionas/Utils/ConfigOptions/GCSConfigOptions.cs) | C# | -8 | 0 | -4 | -12 |
| [Aukcionas/Utils/EmailBody.cs](/Aukcionas/Utils/EmailBody.cs) | C# | -148 | 0 | -10 | -158 |
| [Aukcionas/Utils/Extensions.cs](/Aukcionas/Utils/Extensions.cs) | C# | -17 | 0 | -4 | -21 |
| [Aukcionas/Utils/GetAucReq.cs](/Aukcionas/Utils/GetAucReq.cs) | C# | -17 | 0 | -9 | -26 |
| [Aukcionas/Utils/IDailycoService.cs](/Aukcionas/Utils/IDailycoService.cs) | C# | -8 | 0 | -1 | -9 |
| [Aukcionas/Utils/IEmailService.cs](/Aukcionas/Utils/IEmailService.cs) | C# | -8 | 0 | -2 | -10 |
| [Aukcionas/Utils/PaymentUtils.cs](/Aukcionas/Utils/PaymentUtils.cs) | C# | -58 | 0 | -9 | -67 |
| [Aukcionas/appsettings.Development.json](/Aukcionas/appsettings.Development.json) | JSON | -14 | 0 | -1 | -15 |
| [Aukcionas/appsettings.json](/Aukcionas/appsettings.json) | JSON | -36 | 0 | -1 | -37 |
| [Aukcionas/bin/Debug/net8.0/Aukcionas.deps.json](/Aukcionas/bin/Debug/net8.0/Aukcionas.deps.json) | JSON | -2,668 | 0 | 0 | -2,668 |
| [Aukcionas/bin/Debug/net8.0/Aukcionas.runtimeconfig.json](/Aukcionas/bin/Debug/net8.0/Aukcionas.runtimeconfig.json) | JSON | -21 | 0 | 0 | -21 |
| [Aukcionas/bin/Debug/net8.0/appsettings.Development.json](/Aukcionas/bin/Debug/net8.0/appsettings.Development.json) | JSON | -14 | 0 | -1 | -15 |
| [Aukcionas/bin/Debug/net8.0/appsettings.json](/Aukcionas/bin/Debug/net8.0/appsettings.json) | JSON | -36 | 0 | -1 | -37 |
| [Aukcionas/obj/Aukcionas.csproj.EntityFrameworkCore.targets](/Aukcionas/obj/Aukcionas.csproj.EntityFrameworkCore.targets) | MSBuild | -28 | 0 | -1 | -29 |
| [Aukcionas/obj/Aukcionas.csproj.nuget.dgspec.json](/Aukcionas/obj/Aukcionas.csproj.nuget.dgspec.json) | JSON | -137 | 0 | 0 | -137 |
| [Aukcionas/obj/Aukcionas.csproj.nuget.g.props](/Aukcionas/obj/Aukcionas.csproj.nuget.g.props) | MSBuild | -32 | 0 | 0 | -32 |
| [Aukcionas/obj/Aukcionas.csproj.nuget.g.targets](/Aukcionas/obj/Aukcionas.csproj.nuget.g.targets) | MSBuild | -10 | 0 | 0 | -10 |
| [Aukcionas/obj/Debug/net8.0/.NETCoreApp,Version=v8.0.AssemblyAttributes.cs](/Aukcionas/obj/Debug/net8.0/.NETCoreApp,Version=v8.0.AssemblyAttributes.cs) | C# | -3 | -1 | -1 | -5 |
| [Aukcionas/obj/Debug/net8.0/Aukcionas.AssemblyInfo.cs](/Aukcionas/obj/Debug/net8.0/Aukcionas.AssemblyInfo.cs) | C# | -9 | -10 | -5 | -24 |
| [Aukcionas/obj/Debug/net8.0/Aukcionas.GeneratedMSBuildEditorConfig.editorconfig](/Aukcionas/obj/Debug/net8.0/Aukcionas.GeneratedMSBuildEditorConfig.editorconfig) | EditorConfig | -19 | 0 | -1 | -20 |
| [Aukcionas/obj/Debug/net8.0/Aukcionas.GlobalUsings.g.cs](/Aukcionas/obj/Debug/net8.0/Aukcionas.GlobalUsings.g.cs) | C# | -16 | -1 | -1 | -18 |
| [Aukcionas/obj/Debug/net8.0/Aukcionas.MvcApplicationPartsAssemblyInfo.cs](/Aukcionas/obj/Debug/net8.0/Aukcionas.MvcApplicationPartsAssemblyInfo.cs) | C# | -4 | -10 | -5 | -19 |
| [Aukcionas/obj/Debug/net8.0/staticwebassets.build.json](/Aukcionas/obj/Debug/net8.0/staticwebassets.build.json) | JSON | -11 | 0 | 0 | -11 |
| [Aukcionas/obj/Debug/net8.0/staticwebassets/msbuild.build.Aukcionas.props](/Aukcionas/obj/Debug/net8.0/staticwebassets/msbuild.build.Aukcionas.props) | MSBuild | -3 | 0 | 0 | -3 |
| [Aukcionas/obj/Debug/net8.0/staticwebassets/msbuild.buildMultiTargeting.Aukcionas.props](/Aukcionas/obj/Debug/net8.0/staticwebassets/msbuild.buildMultiTargeting.Aukcionas.props) | MSBuild | -3 | 0 | 0 | -3 |
| [Aukcionas/obj/Debug/net8.0/staticwebassets/msbuild.buildTransitive.Aukcionas.props](/Aukcionas/obj/Debug/net8.0/staticwebassets/msbuild.buildTransitive.Aukcionas.props) | MSBuild | -3 | 0 | 0 | -3 |
| [Aukcionas/obj/project.assets.json](/Aukcionas/obj/project.assets.json) | JSON | -7,370 | 0 | 0 | -7,370 |
| [XUnitTests/AuctionStatusServiceTests.cs](/XUnitTests/AuctionStatusServiceTests.cs) | C# | 27 | 0 | 3 | 30 |
| [XUnitTests/Aukcionas_AdminController.html](/XUnitTests/Aukcionas_AdminController.html) | HTML | 316 | 0 | 8 | 324 |
| [XUnitTests/Aukcionas_AiModel.html](/XUnitTests/Aukcionas_AiModel.html) | HTML | 804 | 0 | 8 | 812 |
| [XUnitTests/Aukcionas_AiModelv2.html](/XUnitTests/Aukcionas_AiModelv2.html) | HTML | 840 | 0 | 8 | 848 |
| [XUnitTests/Aukcionas_AiModelv3.html](/XUnitTests/Aukcionas_AiModelv3.html) | HTML | 839 | 0 | 8 | 847 |
| [XUnitTests/Aukcionas_AiModelv4.html](/XUnitTests/Aukcionas_AiModelv4.html) | HTML | 838 | 0 | 8 | 846 |
| [XUnitTests/Aukcionas_AiModelv5.html](/XUnitTests/Aukcionas_AiModelv5.html) | HTML | 830 | 0 | 8 | 838 |
| [XUnitTests/Aukcionas_AiModelv6.html](/XUnitTests/Aukcionas_AiModelv6.html) | HTML | 903 | 0 | 8 | 911 |
| [XUnitTests/Aukcionas_AiModelv7.html](/XUnitTests/Aukcionas_AiModelv7.html) | HTML | 851 | 0 | 8 | 859 |
| [XUnitTests/Aukcionas_AiModelv8.html](/XUnitTests/Aukcionas_AiModelv8.html) | HTML | 862 | 0 | 8 | 870 |
| [XUnitTests/Aukcionas_Auction.html](/XUnitTests/Aukcionas_Auction.html) | HTML | 302 | 0 | 8 | 310 |
| [XUnitTests/Aukcionas_AuctionController.html](/XUnitTests/Aukcionas_AuctionController.html) | HTML | 725 | 0 | 8 | 733 |
| [XUnitTests/Aukcionas_AuctionForPrediction.html](/XUnitTests/Aukcionas_AuctionForPrediction.html) | HTML | 315 | 0 | 8 | 323 |
| [XUnitTests/Aukcionas_AuctionRatingPrediction.html](/XUnitTests/Aukcionas_AuctionRatingPrediction.html) | HTML | 313 | 0 | 8 | 321 |
| [XUnitTests/Aukcionas_AuctionRecommender.html](/XUnitTests/Aukcionas_AuctionRecommender.html) | HTML | 337 | 0 | 8 | 345 |
| [XUnitTests/Aukcionas_AuctionStatusChecker.html](/XUnitTests/Aukcionas_AuctionStatusChecker.html) | HTML | 205 | 0 | 8 | 213 |
| [XUnitTests/Aukcionas_AuctionStatusService.html](/XUnitTests/Aukcionas_AuctionStatusService.html) | HTML | 264 | 0 | 8 | 272 |
| [XUnitTests/Aukcionas_AuthController.html](/XUnitTests/Aukcionas_AuthController.html) | HTML | 550 | 0 | 8 | 558 |
| [XUnitTests/Aukcionas_AuthDbSeeder.html](/XUnitTests/Aukcionas_AuthDbSeeder.html) | HTML | 224 | 0 | 8 | 232 |
| [XUnitTests/Aukcionas_BiddingHub.html](/XUnitTests/Aukcionas_BiddingHub.html) | HTML | 281 | 0 | 8 | 289 |
| [XUnitTests/Aukcionas_Chat.html](/XUnitTests/Aukcionas_Chat.html) | HTML | 167 | 0 | 8 | 175 |
| [XUnitTests/Aukcionas_ChatController.html](/XUnitTests/Aukcionas_ChatController.html) | HTML | 369 | 0 | 8 | 377 |
| [XUnitTests/Aukcionas_ChatFunction.html](/XUnitTests/Aukcionas_ChatFunction.html) | HTML | 858 | 0 | 8 | 866 |
| [XUnitTests/Aukcionas_ChatFunctionv2.html](/XUnitTests/Aukcionas_ChatFunctionv2.html) | HTML | 1,050 | 0 | 8 | 1,058 |
| [XUnitTests/Aukcionas_ChatFunctionv3.html](/XUnitTests/Aukcionas_ChatFunctionv3.html) | HTML | 990 | 0 | 8 | 998 |
| [XUnitTests/Aukcionas_ChatFunctionv4.html](/XUnitTests/Aukcionas_ChatFunctionv4.html) | HTML | 1,055 | 0 | 8 | 1,063 |
| [XUnitTests/Aukcionas_Chatfunctionv5.html](/XUnitTests/Aukcionas_Chatfunctionv5.html) | HTML | 963 | 0 | 8 | 971 |
| [XUnitTests/Aukcionas_CloudStorageService.html](/XUnitTests/Aukcionas_CloudStorageService.html) | HTML | 272 | 0 | 8 | 280 |
| [XUnitTests/Aukcionas_CollectData.html](/XUnitTests/Aukcionas_CollectData.html) | HTML | 864 | 0 | 8 | 872 |
| [XUnitTests/Aukcionas_Comment.html](/XUnitTests/Aukcionas_Comment.html) | HTML | 258 | 0 | 8 | 266 |
| [XUnitTests/Aukcionas_DailycoService.html](/XUnitTests/Aukcionas_DailycoService.html) | HTML | 277 | 0 | 8 | 285 |
| [XUnitTests/Aukcionas_DataContext.html](/XUnitTests/Aukcionas_DataContext.html) | HTML | 196 | 0 | 8 | 204 |
| [XUnitTests/Aukcionas_DataContextModelSnapshot.html](/XUnitTests/Aukcionas_DataContextModelSnapshot.html) | HTML | 887 | 0 | 8 | 895 |
| [XUnitTests/Aukcionas_DecodedPaymentToken.html](/XUnitTests/Aukcionas_DecodedPaymentToken.html) | HTML | 159 | 0 | 8 | 167 |
| [XUnitTests/Aukcionas_EmailBody.html](/XUnitTests/Aukcionas_EmailBody.html) | HTML | 335 | 0 | 8 | 343 |
| [XUnitTests/Aukcionas_EmailConfirm.html](/XUnitTests/Aukcionas_EmailConfirm.html) | HTML | 643 | 0 | 8 | 651 |
| [XUnitTests/Aukcionas_EmailConfirmfix.html](/XUnitTests/Aukcionas_EmailConfirmfix.html) | HTML | 640 | 0 | 8 | 648 |
| [XUnitTests/Aukcionas_EmailConfirmfixv2.html](/XUnitTests/Aukcionas_EmailConfirmfixv2.html) | HTML | 638 | 0 | 8 | 646 |
| [XUnitTests/Aukcionas_EmailModel.html](/XUnitTests/Aukcionas_EmailModel.html) | HTML | 187 | 0 | 8 | 195 |
| [XUnitTests/Aukcionas_EmailService.html](/XUnitTests/Aukcionas_EmailService.html) | HTML | 215 | 0 | 8 | 223 |
| [XUnitTests/Aukcionas_Extensions.html](/XUnitTests/Aukcionas_Extensions.html) | HTML | 186 | 0 | 8 | 194 |
| [XUnitTests/Aukcionas_FixPRoblem.html](/XUnitTests/Aukcionas_FixPRoblem.html) | HTML | 709 | 0 | 8 | 717 |
| [XUnitTests/Aukcionas_ForumRestUser.html](/XUnitTests/Aukcionas_ForumRestUser.html) | HTML | 209 | 0 | 8 | 217 |
| [XUnitTests/Aukcionas_ForumRoles.html](/XUnitTests/Aukcionas_ForumRoles.html) | HTML | 176 | 0 | 8 | 184 |
| [XUnitTests/Aukcionas_GCSConfigOptions.html](/XUnitTests/Aukcionas_GCSConfigOptions.html) | HTML | 162 | 0 | 8 | 170 |
| [XUnitTests/Aukcionas_GetAucReq.html](/XUnitTests/Aukcionas_GetAucReq.html) | HTML | 194 | 0 | 8 | 202 |
| [XUnitTests/Aukcionas_Init.html](/XUnitTests/Aukcionas_Init.html) | HTML | 612 | 0 | 8 | 620 |
| [XUnitTests/Aukcionas_JwtTokenService.html](/XUnitTests/Aukcionas_JwtTokenService.html) | HTML | 214 | 0 | 8 | 222 |
| [XUnitTests/Aukcionas_LivestreamController.html](/XUnitTests/Aukcionas_LivestreamController.html) | HTML | 255 | 0 | 8 | 263 |
| [XUnitTests/Aukcionas_LoginDto.html](/XUnitTests/Aukcionas_LoginDto.html) | HTML | 162 | 0 | 8 | 170 |
| [XUnitTests/Aukcionas_Message.html](/XUnitTests/Aukcionas_Message.html) | HTML | 169 | 0 | 8 | 177 |
| [XUnitTests/Aukcionas_PasswordReset.html](/XUnitTests/Aukcionas_PasswordReset.html) | HTML | 636 | 0 | 8 | 644 |
| [XUnitTests/Aukcionas_PayPalAccessToken.html](/XUnitTests/Aukcionas_PayPalAccessToken.html) | HTML | 165 | 0 | 8 | 173 |
| [XUnitTests/Aukcionas_Payment.2.html](/XUnitTests/Aukcionas_Payment.2.html) | HTML | 193 | 0 | 8 | 201 |
| [XUnitTests/Aukcionas_Payment.html](/XUnitTests/Aukcionas_Payment.html) | HTML | 193 | 0 | 8 | 201 |
| [XUnitTests/Aukcionas_PaymentController.html](/XUnitTests/Aukcionas_PaymentController.html) | HTML | 477 | 0 | 8 | 485 |
| [XUnitTests/Aukcionas_PaymentUtils.html](/XUnitTests/Aukcionas_PaymentUtils.html) | HTML | 236 | 0 | 8 | 244 |
| [XUnitTests/Aukcionas_PaymentV3.html](/XUnitTests/Aukcionas_PaymentV3.html) | HTML | 777 | 0 | 8 | 785 |
| [XUnitTests/Aukcionas_PaymentV4.html](/XUnitTests/Aukcionas_PaymentV4.html) | HTML | 770 | 0 | 8 | 778 |
| [XUnitTests/Aukcionas_PaypalConfirmation.html](/XUnitTests/Aukcionas_PaypalConfirmation.html) | HTML | 711 | 0 | 8 | 719 |
| [XUnitTests/Aukcionas_Photos.html](/XUnitTests/Aukcionas_Photos.html) | HTML | 811 | 0 | 8 | 819 |
| [XUnitTests/Aukcionas_RecommendationService.html](/XUnitTests/Aukcionas_RecommendationService.html) | HTML | 199 | 0 | 8 | 207 |
| [XUnitTests/Aukcionas_Recommendations.html](/XUnitTests/Aukcionas_Recommendations.html) | HTML | 176 | 0 | 8 | 184 |
| [XUnitTests/Aukcionas_RegisterUserDto.html](/XUnitTests/Aukcionas_RegisterUserDto.html) | HTML | 162 | 0 | 8 | 170 |
| [XUnitTests/Aukcionas_Report.2.html](/XUnitTests/Aukcionas_Report.2.html) | HTML | 169 | 0 | 8 | 177 |
| [XUnitTests/Aukcionas_Report.html](/XUnitTests/Aukcionas_Report.html) | HTML | 169 | 0 | 8 | 177 |
| [XUnitTests/Aukcionas_ReportController.html](/XUnitTests/Aukcionas_ReportController.html) | HTML | 347 | 0 | 8 | 355 |
| [XUnitTests/Aukcionas_ResetPasswordDto.html](/XUnitTests/Aukcionas_ResetPasswordDto.html) | HTML | 163 | 0 | 8 | 171 |
| [XUnitTests/Aukcionas_ResourceOwnerAuthorizationHandler.html](/XUnitTests/Aukcionas_ResourceOwnerAuthorizationHandler.html) | HTML | 186 | 0 | 8 | 194 |
| [XUnitTests/Aukcionas_RomovedPaypalEmail.html](/XUnitTests/Aukcionas_RomovedPaypalEmail.html) | HTML | 708 | 0 | 8 | 716 |
| [XUnitTests/Aukcionas_SuccessfulLoginDto.html](/XUnitTests/Aukcionas_SuccessfulLoginDto.html) | HTML | 162 | 0 | 8 | 170 |
| [XUnitTests/Aukcionas_UpdatePayment.html](/XUnitTests/Aukcionas_UpdatePayment.html) | HTML | 758 | 0 | 8 | 766 |
| [XUnitTests/Aukcionas_UserDto.html](/XUnitTests/Aukcionas_UserDto.html) | HTML | 162 | 0 | 8 | 170 |
| [XUnitTests/Aukcionas_UserInfo.html](/XUnitTests/Aukcionas_UserInfo.html) | HTML | 862 | 0 | 8 | 870 |
| [XUnitTests/Aukcionas_UserRecommendations.html](/XUnitTests/Aukcionas_UserRecommendations.html) | HTML | 161 | 0 | 8 | 169 |
| [XUnitTests/Aukcionas_auctionComments.html](/XUnitTests/Aukcionas_auctionComments.html) | HTML | 702 | 0 | 8 | 710 |
| [XUnitTests/Aukcionas_auctionCommentsv2.html](/XUnitTests/Aukcionas_auctionCommentsv2.html) | HTML | 731 | 0 | 8 | 739 |
| [XUnitTests/Aukcionas_auctionCommentsv3.html](/XUnitTests/Aukcionas_auctionCommentsv3.html) | HTML | 675 | 0 | 8 | 683 |
| [XUnitTests/Aukcionas_auctionCommentsv4.html](/XUnitTests/Aukcionas_auctionCommentsv4.html) | HTML | 675 | 0 | 8 | 683 |
| [XUnitTests/Aukcionas_auctionCommentsv5.html](/XUnitTests/Aukcionas_auctionCommentsv5.html) | HTML | 675 | 0 | 8 | 683 |
| [XUnitTests/Aukcionas_auctionLikes.html](/XUnitTests/Aukcionas_auctionLikes.html) | HTML | 634 | 0 | 8 | 642 |
| [XUnitTests/Aukcionas_auctionPictures.html](/XUnitTests/Aukcionas_auctionPictures.html) | HTML | 713 | 0 | 8 | 721 |
| [XUnitTests/Aukcionas_user_payment_info.html](/XUnitTests/Aukcionas_user_payment_info.html) | HTML | 918 | 0 | 8 | 926 |
| [XUnitTests/BiddingHubTests.cs](/XUnitTests/BiddingHubTests.cs) | C# | 190 | 0 | 9 | 199 |
| [XUnitTests/DailycoServiceTests.cs](/XUnitTests/DailycoServiceTests.cs) | C# | 18 | 0 | 3 | 21 |
| [XUnitTests/EmailBodyTests.cs](/XUnitTests/EmailBodyTests.cs) | C# | 215 | 0 | 13 | 228 |
| [XUnitTests/ExtensionsTests.cs](/XUnitTests/ExtensionsTests.cs) | C# | 80 | 0 | 7 | 87 |
| [XUnitTests/GetAuqReqTests.cs](/XUnitTests/GetAuqReqTests.cs) | C# | 73 | 0 | 7 | 80 |
| [XUnitTests/GlobalUsings.cs](/XUnitTests/GlobalUsings.cs) | C# | 1 | 0 | 0 | 1 |
| [XUnitTests/PaymentTests.cs](/XUnitTests/PaymentTests.cs) | C# | 11 | 0 | 2 | 13 |
| [XUnitTests/PaymentUtilsTest.cs](/XUnitTests/PaymentUtilsTest.cs) | C# | 93 | 0 | 9 | 102 |
| [XUnitTests/XUnitTests.csproj](/XUnitTests/XUnitTests.csproj) | MSBuild | 31 | 0 | 5 | 36 |
| [XUnitTests/bin/Debug/net8.0/Aukcionas.deps.json](/XUnitTests/bin/Debug/net8.0/Aukcionas.deps.json) | JSON | 2,677 | 0 | 0 | 2,677 |
| [XUnitTests/bin/Debug/net8.0/Aukcionas.runtimeconfig.json](/XUnitTests/bin/Debug/net8.0/Aukcionas.runtimeconfig.json) | JSON | 21 | 0 | 0 | 21 |
| [XUnitTests/bin/Debug/net8.0/MvcTestingAppManifest.json](/XUnitTests/bin/Debug/net8.0/MvcTestingAppManifest.json) | JSON | 3 | 0 | 0 | 3 |
| [XUnitTests/bin/Debug/net8.0/XUnitTests.deps.json](/XUnitTests/bin/Debug/net8.0/XUnitTests.deps.json) | JSON | 6,840 | 0 | 0 | 6,840 |
| [XUnitTests/bin/Debug/net8.0/XUnitTests.runtimeconfig.json](/XUnitTests/bin/Debug/net8.0/XUnitTests.runtimeconfig.json) | JSON | 19 | 0 | 0 | 19 |
| [XUnitTests/bin/Debug/net8.0/appsettings.Development.json](/XUnitTests/bin/Debug/net8.0/appsettings.Development.json) | JSON | 14 | 0 | 1 | 15 |
| [XUnitTests/bin/Debug/net8.0/appsettings.json](/XUnitTests/bin/Debug/net8.0/appsettings.json) | JSON | 36 | 0 | 1 | 37 |
| [XUnitTests/class.js](/XUnitTests/class.js) | JavaScript | 169 | 14 | 35 | 218 |
| [XUnitTests/coverage.opencover.xml](/XUnitTests/coverage.opencover.xml) | XML | 5,450 | 0 | 0 | 5,450 |
| [XUnitTests/icon_cog.svg](/XUnitTests/icon_cog.svg) | XML | 1 | 0 | 0 | 1 |
| [XUnitTests/icon_cog_dark.svg](/XUnitTests/icon_cog_dark.svg) | XML | 1 | 0 | 0 | 1 |
| [XUnitTests/icon_cube.svg](/XUnitTests/icon_cube.svg) | XML | 2 | 0 | 0 | 2 |
| [XUnitTests/icon_cube_dark.svg](/XUnitTests/icon_cube_dark.svg) | XML | 1 | 0 | 0 | 1 |
| [XUnitTests/icon_down-dir_active.svg](/XUnitTests/icon_down-dir_active.svg) | XML | 2 | 0 | 0 | 2 |
| [XUnitTests/icon_down-dir_active_dark.svg](/XUnitTests/icon_down-dir_active_dark.svg) | XML | 1 | 0 | 0 | 1 |
| [XUnitTests/icon_fork.svg](/XUnitTests/icon_fork.svg) | XML | 2 | 0 | 0 | 2 |
| [XUnitTests/icon_fork_dark.svg](/XUnitTests/icon_fork_dark.svg) | XML | 1 | 0 | 0 | 1 |
| [XUnitTests/icon_info-circled.svg](/XUnitTests/icon_info-circled.svg) | XML | 2 | 0 | 0 | 2 |
| [XUnitTests/icon_info-circled_dark.svg](/XUnitTests/icon_info-circled_dark.svg) | XML | 2 | 0 | 0 | 2 |
| [XUnitTests/icon_minus.svg](/XUnitTests/icon_minus.svg) | XML | 2 | 0 | 0 | 2 |
| [XUnitTests/icon_minus_dark.svg](/XUnitTests/icon_minus_dark.svg) | XML | 1 | 0 | 0 | 1 |
| [XUnitTests/icon_plus.svg](/XUnitTests/icon_plus.svg) | XML | 2 | 0 | 0 | 2 |
| [XUnitTests/icon_plus_dark.svg](/XUnitTests/icon_plus_dark.svg) | XML | 1 | 0 | 0 | 1 |
| [XUnitTests/icon_search-minus.svg](/XUnitTests/icon_search-minus.svg) | XML | 2 | 0 | 0 | 2 |
| [XUnitTests/icon_search-minus_dark.svg](/XUnitTests/icon_search-minus_dark.svg) | XML | 1 | 0 | 0 | 1 |
| [XUnitTests/icon_search-plus.svg](/XUnitTests/icon_search-plus.svg) | XML | 2 | 0 | 0 | 2 |
| [XUnitTests/icon_search-plus_dark.svg](/XUnitTests/icon_search-plus_dark.svg) | XML | 1 | 0 | 0 | 1 |
| [XUnitTests/icon_sponsor.svg](/XUnitTests/icon_sponsor.svg) | XML | 2 | 0 | 0 | 2 |
| [XUnitTests/icon_star.svg](/XUnitTests/icon_star.svg) | XML | 2 | 0 | 0 | 2 |
| [XUnitTests/icon_star_dark.svg](/XUnitTests/icon_star_dark.svg) | XML | 2 | 0 | 0 | 2 |
| [XUnitTests/icon_up-dir.svg](/XUnitTests/icon_up-dir.svg) | XML | 2 | 0 | 0 | 2 |
| [XUnitTests/icon_up-dir_active.svg](/XUnitTests/icon_up-dir_active.svg) | XML | 2 | 0 | 0 | 2 |
| [XUnitTests/icon_wrench.svg](/XUnitTests/icon_wrench.svg) | XML | 2 | 0 | 0 | 2 |
| [XUnitTests/icon_wrench_dark.svg](/XUnitTests/icon_wrench_dark.svg) | XML | 1 | 0 | 0 | 1 |
| [XUnitTests/index.htm](/XUnitTests/index.htm) | HTML | 211 | 0 | 8 | 219 |
| [XUnitTests/index.html](/XUnitTests/index.html) | HTML | 211 | 0 | 8 | 219 |
| [XUnitTests/main.js](/XUnitTests/main.js) | JavaScript | 271 | 14 | 46 | 331 |
| [XUnitTests/obj/Debug/net8.0/.NETCoreApp,Version=v8.0.AssemblyAttributes.cs](/XUnitTests/obj/Debug/net8.0/.NETCoreApp,Version=v8.0.AssemblyAttributes.cs) | C# | 3 | 1 | 1 | 5 |
| [XUnitTests/obj/Debug/net8.0/MvcTestingAppManifest.json](/XUnitTests/obj/Debug/net8.0/MvcTestingAppManifest.json) | JSON | 3 | 0 | 0 | 3 |
| [XUnitTests/obj/Debug/net8.0/XUnitTests.AssemblyInfo.cs](/XUnitTests/obj/Debug/net8.0/XUnitTests.AssemblyInfo.cs) | C# | 9 | 10 | 5 | 24 |
| [XUnitTests/obj/Debug/net8.0/XUnitTests.GeneratedMSBuildEditorConfig.editorconfig](/XUnitTests/obj/Debug/net8.0/XUnitTests.GeneratedMSBuildEditorConfig.editorconfig) | EditorConfig | 13 | 0 | 1 | 14 |
| [XUnitTests/obj/Debug/net8.0/XUnitTests.GlobalUsings.g.cs](/XUnitTests/obj/Debug/net8.0/XUnitTests.GlobalUsings.g.cs) | C# | 7 | 1 | 1 | 9 |
| [XUnitTests/obj/XUnitTests.csproj.nuget.dgspec.json](/XUnitTests/obj/XUnitTests.csproj.nuget.dgspec.json) | JSON | 255 | 0 | 0 | 255 |
| [XUnitTests/obj/XUnitTests.csproj.nuget.g.props](/XUnitTests/obj/XUnitTests.csproj.nuget.g.props) | MSBuild | 33 | 0 | 0 | 33 |
| [XUnitTests/obj/XUnitTests.csproj.nuget.g.targets](/XUnitTests/obj/XUnitTests.csproj.nuget.g.targets) | MSBuild | 16 | 0 | 0 | 16 |
| [XUnitTests/obj/project.assets.json](/XUnitTests/obj/project.assets.json) | JSON | 8,583 | 0 | 0 | 8,583 |
| [XUnitTests/report.css](/XUnitTests/report.css) | CSS | 700 | 0 | 85 | 785 |

[Summary](results.md) / [Details](details.md) / [Diff Summary](diff.md) / Diff Details