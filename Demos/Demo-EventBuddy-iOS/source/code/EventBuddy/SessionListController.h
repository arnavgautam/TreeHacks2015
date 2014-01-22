//
//  SessionListController.h
//  EventBuddy
//
//  Created by Chris Risner on 11/4/12.
//  Copyright (c) 2012 Microsoft. All rights reserved.
//

#import <UIKit/UIKit.h>
#import "NewSessionViewController.h"
#import "PullRefreshTableViewController.h"

@interface SessionListController : PullRefreshTableViewController <NewSessionViewControllerDelegate>

@property (strong, nonatomic) NSString *eventName;
@property (strong, nonatomic) NSNumber *eventId;
-(void) refreshData;

@end
