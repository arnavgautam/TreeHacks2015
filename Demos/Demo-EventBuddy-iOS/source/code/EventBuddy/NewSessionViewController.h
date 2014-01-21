//
//  NewSessionViewController.h
//  EventBuddy
//
//  Created by Chris Risner on 11/6/12.
//  Copyright (c) 2012 Microsoft. All rights reserved.
//

#import <UIKit/UIKit.h>

@class NewSessionViewController;

@protocol NewSessionViewControllerDelegate <NSObject>
- (void)refreshData;
@end

@interface NewSessionViewController : UIViewController <UITextFieldDelegate>{
    UIActivityIndicatorView *activityIndicator;
}

@property (strong, nonatomic) NSNumber* eventId;
@property (nonatomic, weak) id <NewSessionViewControllerDelegate> delegate;
@property (weak, nonatomic) IBOutlet UITextField *txtName;
@property (weak, nonatomic) IBOutlet UITextField *txtTwitter;
@property (weak, nonatomic) IBOutlet UITextView *txtDescription;
@property (weak, nonatomic) IBOutlet UITextField *txtRoom;
@property (weak, nonatomic) IBOutlet UITextField *txtStartDate;
@property (weak, nonatomic) IBOutlet UITextField *txtEndDate;
- (IBAction)tappedSaveSession:(id)sender;

@end
