//
//  EventListController.m
//  EventBuddy
//
//  Created by Chris Risner on 11/4/12.
//  Copyright (c) 2012 Microsoft. All rights reserved.
//

#import "EventListController.h"
//#import <WindowsAzureMobileServices/WindowsAzureMobileServices.h>
#import "EventBuddyService.h"
#import "SessionListController.h"
#import "NewEventViewController.h"

@interface EventListController ()

@property (strong, nonatomic) EventBuddyService *ebService;

@end

@implementation EventListController

BOOL didLogout;

- (id)initWithStyle:(UITableViewStyle)style
{
    self = [super initWithStyle:style];
    if (self) {
        // Custom initialization
    }
    didLogout = false;
    return self;
}

-(void)setbackground {
    //Resize background
    UIGraphicsBeginImageContext(self.view.frame.size);
    [[UIImage imageNamed:@"wpbackground-notop.png"] drawInRect:self.view.bounds];
    UIImage *image = UIGraphicsGetImageFromCurrentImageContext();
    UIGraphicsEndImageContext();
    
    self.view.backgroundColor = [UIColor colorWithPatternImage:image];
}

-(void)didRotateFromInterfaceOrientation:(UIInterfaceOrientation)fromInterfaceOrientation {
    [self setbackground];
}

- (void)viewDidLoad
{
    [super viewDidLoad];

    //[self setbackground];
    
    self.navigationItem.backBarButtonItem = [[UIBarButtonItem alloc] initWithTitle:@"Events" style:UIBarButtonItemStylePlain target:nil action:nil];
    
    self.ebService = [EventBuddyService getInstance];
    // Uncomment the following line to preserve selection between presentations.
    // self.clearsSelectionOnViewWillAppear = NO;
 
    // Uncomment the following line to display an Edit button in the navigation bar for this view controller.
    // self.navigationItem.rightBarButtonItem = self.editButtonItem;
}
- (void)viewDidAppear:(BOOL)animated
{
    [self setbackground];
    // If user is already logged in, no need to ask for auth
    if (self.ebService.client.currentUser == nil)
    {
        // We want the login view to be presented after the this run loop has completed
        // Here we use a delay to ensure this.
        [self performSelector:@selector(login) withObject:self afterDelay:0.1];
    }
}

- (void) login
{
    UINavigationController *controller =
    
    
    [self.ebService.client
     loginViewControllerWithProvider:@"twitter"
     completion:^(MSUser *user, NSError *error) {    
         if (error) {
             NSLog(@"Authentication Error: %@", error);
             // Note that error.code == -1503 indicates
             // that the user cancelled the dialog
         } else {
             // No error, so load the data
             didLogout = NO;
            self.btnLogout.title = @"Logout";
             self.btnAddEvent.enabled = YES;
             [self refreshData];
         }
         [self dismissViewControllerAnimated:YES completion:nil];
     }];
    [self presentViewController:controller animated:YES completion:nil];
}

- (IBAction)tappedLogout:(id)sender {
    if (!didLogout) {
        [self.ebService.client logout];
        didLogout = YES;
        for (NSHTTPCookie *value in [NSHTTPCookieStorage sharedHTTPCookieStorage].cookies) {
            [[NSHTTPCookieStorage sharedHTTPCookieStorage] deleteCookie:value];
        }
        self.btnLogout.title = @"Login";
        self.ebService.events = [[NSMutableArray alloc] init];
        [self.tableView reloadData];
        self.btnAddEvent.enabled = NO;
    }
    else {
        [self login];
    }
}

-(void) refreshData {
    [self.ebService refreshEventsOnSuccess:^{
        [self.tableView reloadData];
        [self performSelector:@selector(stopLoading)];
    }];
}

- (void)refresh {
    [self refreshData];
}

- (void)didReceiveMemoryWarning
{
    [super didReceiveMemoryWarning];
    // Dispose of any resources that can be recreated.
}

#pragma mark - Table view data source

- (NSInteger)numberOfSectionsInTableView:(UITableView *)tableView
{
    return 1;
}

- (NSInteger)tableView:(UITableView *)tableView numberOfRowsInSection:(NSInteger)section
{
    return [self.ebService.events count];
}

- (UITableViewCell *)tableView:(UITableView *)tableView cellForRowAtIndexPath:(NSIndexPath *)indexPath
{
    static NSString *CellIdentifier = @"Cell";
    UITableViewCell *cell = [tableView dequeueReusableCellWithIdentifier:CellIdentifier forIndexPath:indexPath];
    
    // Configure the cell...
    NSDictionary *event = [self.ebService.events objectAtIndex:indexPath.row];
    UILabel *eventNameLabel = (UILabel *)[cell viewWithTag:1];
    eventNameLabel.textColor = [UIColor blackColor];
    eventNameLabel.text = [event objectForKey:@"name"];
    
    UILabel *eventDescriptionLabel = (UILabel *)[cell viewWithTag:2];
    eventDescriptionLabel.textColor = [UIColor redColor];
    eventDescriptionLabel.text = [event objectForKey:@"description"];

    NSDateFormatter* formatter = [[NSDateFormatter alloc] init];
    [formatter setDateFormat:@"MM.dd.yyyy"];
    
    UILabel *startLabel = (UILabel *)[cell viewWithTag:3];
    startLabel.textColor = [UIColor blueColor];
    NSDate *startDate = [event objectForKey:@"start"];
    startLabel.text = [NSString stringWithFormat:@"Starts:%@ | ", [formatter stringFromDate:startDate]];

    UILabel *endLabel = (UILabel *)[cell viewWithTag:4];
    endLabel.textColor = [UIColor blueColor];
    NSDate *endDate = [event objectForKey:@"end"];
    endLabel.text = [NSString stringWithFormat:@"Ends:%@", [formatter stringFromDate:endDate]];
    //    endLabel.text = [event objectForKey:@"end"];
//    
    return cell;
}

/*
// Override to support conditional editing of the table view.
- (BOOL)tableView:(UITableView *)tableView canEditRowAtIndexPath:(NSIndexPath *)indexPath
{
    // Return NO if you do not want the specified item to be editable.
    return YES;
}
*/

/*
// Override to support editing the table view.
- (void)tableView:(UITableView *)tableView commitEditingStyle:(UITableViewCellEditingStyle)editingStyle forRowAtIndexPath:(NSIndexPath *)indexPath
{
    if (editingStyle == UITableViewCellEditingStyleDelete) {
        // Delete the row from the data source
        [tableView deleteRowsAtIndexPaths:@[indexPath] withRowAnimation:UITableViewRowAnimationFade];
    }   
    else if (editingStyle == UITableViewCellEditingStyleInsert) {
        // Create a new instance of the appropriate class, insert it into the array, and add a new row to the table view
    }   
}
*/

/*
// Override to support rearranging the table view.
- (void)tableView:(UITableView *)tableView moveRowAtIndexPath:(NSIndexPath *)fromIndexPath toIndexPath:(NSIndexPath *)toIndexPath
{
}
*/

/*
// Override to support conditional rearranging of the table view.
- (BOOL)tableView:(UITableView *)tableView canMoveRowAtIndexPath:(NSIndexPath *)indexPath
{
    // Return NO if you do not want the item to be re-orderable.
    return YES;
}
*/

#pragma mark - Table view delegate

- (void)tableView:(UITableView *)tableView didSelectRowAtIndexPath:(NSIndexPath *)indexPath
{
    // Navigation logic may go here. Create and push another view controller.
    /*
     <#DetailViewController#> *detailViewController = [[<#DetailViewController#> alloc] initWithNibName:@"<#Nib name#>" bundle:nil];
     // ...
     // Pass the selected object to the new view controller.
     [self.navigationController pushViewController:detailViewController animated:YES];
     */
}

- (void)prepareForSegue:(UIStoryboardSegue *)segue sender:(id)sender {
    if ([segue.identifier isEqualToString:@"viewEventSessions"]) {
        UITableViewCell *cell = (UITableViewCell *)sender;
        UILabel *eventNameLabel = (UILabel *)[cell viewWithTag:1];
        SessionListController *sessionListViewController =
        segue.destinationViewController;
		sessionListViewController.eventName = eventNameLabel.text;
        for (id event in self.ebService.events) {
            NSString *name = [event objectForKey:@"name"];
            if ([name isEqualToString:sessionListViewController.eventName]) {
                sessionListViewController.eventId = (NSNumber *)[event objectForKey:@"id"];
            }
        }
    } else if ([segue.identifier isEqualToString:@"newEventSegue"]) {
        NewEventViewController *newEventViewController = segue.destinationViewController;
        newEventViewController.delegate = self;
    }
}

@end
