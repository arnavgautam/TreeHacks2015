//
//  NewEventViewController.h
//  EventBuddy
//
//  Created by Chris Risner on 11/6/12.
//  Copyright (c) 2012 Microsoft. All rights reserved.
//

#import <UIKit/UIKit.h>

@class NewEventViewController;

@protocol NewEventViewControllerDelegate <NSObject>
- (void)refreshData;
@end

@interface NewEventViewController : UIViewController <UITextFieldDelegate> {
    UIActivityIndicatorView *activityIndicator;	
}


@property (nonatomic, weak) id <NewEventViewControllerDelegate> delegate;
@property (weak, nonatomic) IBOutlet UITextField *txtTitle;
@property (weak, nonatomic) IBOutlet UITextView *txtDescription;
@property (weak, nonatomic) IBOutlet UITextField *txtStartDate;
@property (weak, nonatomic) IBOutlet UITextField *txtEndDate;
- (IBAction)tappedSaveEvent:(id)sender;

@end
