//
//  SessionDetailsController.h
//  EventBuddy
//
//  Created by Chris Risner on 11/4/12.
//  Copyright (c) 2012 Microsoft. All rights reserved.
//

#import <UIKit/UIKit.h>

@interface SessionDetailsController : UIViewController

@property (strong, nonatomic) NSString *eventName;
@property (strong, nonatomic) NSString *sessionName;

@property (strong, nonatomic) NSDictionary *session;
@property (weak, nonatomic) IBOutlet UILabel *lblSessionName;
@property (weak, nonatomic) IBOutlet UILabel *lblSpeakerName;
@property (weak, nonatomic) IBOutlet UILabel *lblDate;
@property (weak, nonatomic) IBOutlet UILabel *lblTime;
@property (weak, nonatomic) IBOutlet UIImageView *imgSession;
@property (weak, nonatomic) IBOutlet UILabel *lblDescription;
@property (weak, nonatomic) IBOutlet UIImageView *imgDocument;
@property (weak, nonatomic) IBOutlet UILabel *lblDocument;
- (IBAction)tappedStarOne:(id)sender;
- (IBAction)tappedStarTwo:(id)sender;
- (IBAction)tappedStarThree:(id)sender;
- (IBAction)tappeStarFour:(id)sender;
- (IBAction)tappedStarFive:(id)sender;
@property (weak, nonatomic) IBOutlet UIButton *btnStarOne;
@property (weak, nonatomic) IBOutlet UIButton *btnStarTwo;
@property (weak, nonatomic) IBOutlet UIButton *btnStarThree;
@property (weak, nonatomic) IBOutlet UIButton *btnStarFour;
@property (weak, nonatomic) IBOutlet UIButton *btnStarFive;
- (IBAction)tappedViewDocument:(id)sender;


@end
