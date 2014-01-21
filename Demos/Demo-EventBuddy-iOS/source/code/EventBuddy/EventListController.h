//
//  EventListController.h
//  EventBuddy
//
//  Created by Chris Risner on 11/4/12.
//  Copyright (c) 2012 Microsoft. All rights reserved.
//

#import <UIKit/UIKit.h>
#import "NewEventViewController.h"
#import "PullRefreshTableViewController.h"

@interface EventListController : PullRefreshTableViewController <NewEventViewControllerDelegate>
- (IBAction)tappedLogout:(id)sender;
@property (weak, nonatomic) IBOutlet UIBarButtonItem *btnLogout;
@property (weak, nonatomic) IBOutlet UIBarButtonItem *btnAddEvent;

-(void) refreshData;
@end
