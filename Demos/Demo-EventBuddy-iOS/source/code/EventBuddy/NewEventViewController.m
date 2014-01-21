//
//  NewEventViewController.m
//  EventBuddy
//
//  Created by Chris Risner on 11/6/12.
//  Copyright (c) 2012 Microsoft. All rights reserved.
//

#import "NewEventViewController.h"
#import "EventListController.h"
#import "EventBuddyService.h"

@interface NewEventViewController ()

@end

@implementation NewEventViewController

- (id)initWithNibName:(NSString *)nibNameOrNil bundle:(NSBundle *)nibBundleOrNil
{
    self = [super initWithNibName:nibNameOrNil bundle:nibBundleOrNil];
    if (self) {
        // Custom initialization
    }
    return self;
}

- (void)viewDidLoad
{
    [super viewDidLoad];
	// Do any additional setup after loading the view.
    
    //Set delegate on text fields
    self.txtTitle.delegate = self;
    self.txtStartDate.delegate = self;
    self.txtEndDate.delegate = self;
    
    NSDate *now = [NSDate date];
    
    NSDateFormatter *dateFormatter = [[NSDateFormatter alloc] init];
//    [dateFormatter setDateStyle:NSDateFormatterFullStyle];
    [dateFormatter setDateFormat:@"MM/dd/yyyy hh:mm aaa"];
    self.txtStartDate.text = [dateFormatter stringFromDate:now];
    
//    NSTimeInterval secondsInEightHours = 8 * 60 * 60;
//    NSDate *dateEightHoursAhead = [mydate dateByAddingTimeInterval:secondsInEightHours];
    
    now = [now dateByAddingTimeInterval:48*60*60];
    self.txtEndDate.text = [dateFormatter stringFromDate:now];
    
}

-(void)setBackground{
    //Resize background
    UIGraphicsBeginImageContext(self.view.frame.size);
    [[UIImage imageNamed:@"wpbackground-notop.png"] drawInRect:self.view.bounds];
    UIImage *image = UIGraphicsGetImageFromCurrentImageContext();
    UIGraphicsEndImageContext();
    
    self.view.backgroundColor = [UIColor colorWithPatternImage:image];
}

-(void)didRotateFromInterfaceOrientation:(UIInterfaceOrientation)fromInterfaceOrientation {
    [self setBackground];
}

-(void) viewWillAppear:(BOOL)animated {
    [self setBackground];
}

- (void)didReceiveMemoryWarning
{
    [super didReceiveMemoryWarning];
    // Dispose of any resources that can be recreated.
}

- (BOOL)textFieldShouldReturn:(UITextField *)textField {
    [textField resignFirstResponder];
    return YES;
}

- (IBAction)tappedSaveEvent:(id)sender {
    
    NSDateFormatter *dateFormatter = [[NSDateFormatter alloc] init];
    [dateFormatter setDateFormat:@"MM/dd/yyyy hh:mm aaa"];
    
    NSDate *startDate = [dateFormatter dateFromString:self.txtStartDate.text];
    NSDate *endDate = [dateFormatter dateFromString:self.txtEndDate.text];
    
    if ([self.txtTitle.text length]==0) {
        UIAlertView *alert = [[UIAlertView alloc] initWithTitle:@"Error" message:@"Enter an event title" delegate: self cancelButtonTitle: nil otherButtonTitles: @"OK",nil, nil];
        [alert show];
        return;
    }    
    if (!startDate || !endDate) {
        UIAlertView *alert = [[UIAlertView alloc] initWithTitle:@"Error" message:@"Enter a correct date" delegate: self cancelButtonTitle: nil otherButtonTitles: @"OK",nil, nil];
        [alert show];
        return;
    }
    
    //Save the Event
    NSDictionary *eventItem = @{
        @"name" : self.txtTitle.text
    , @"description" : self.txtDescription.text
    , @"start" : startDate
    , @"end" : endDate};
    
    //Activity indicators
    activityIndicator = [[UIActivityIndicatorView alloc]initWithActivityIndicatorStyle:UIActivityIndicatorViewStyleGray];
	activityIndicator.frame = CGRectMake(0.0, 0.0, 40.0, 40.0);
	activityIndicator.center = self.view.center;
	[self.view addSubview: activityIndicator];
    [UIApplication sharedApplication].networkActivityIndicatorVisible = YES;
	[activityIndicator startAnimating];
    
    EventBuddyService *ebService = [EventBuddyService getInstance];
    [ebService saveEvent:eventItem completion:^(NSUInteger index) {
        [self.delegate refreshData];
        // finished loading, hide the activity indicator in the status bar
        [UIApplication sharedApplication].networkActivityIndicatorVisible = NO;
        [activityIndicator stopAnimating];
        
        //Return to the event list
        [self.navigationController popViewControllerAnimated:YES];
    }]; 
}
@end
