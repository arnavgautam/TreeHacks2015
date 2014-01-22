//
//  SessionListController.m
//  EventBuddy
//
//  Created by Chris Risner on 11/4/12.
//  Copyright (c) 2012 Microsoft. All rights reserved.
//

#import "SessionListController.h"
#import "EventBuddyService.h"
#import "SessionDetailsController.h"
#import "NewSessionViewController.h"

@interface SessionListController ()

@property (strong, nonatomic) EventBuddyService *ebService;

@end

@implementation SessionListController

- (id)initWithStyle:(UITableViewStyle)style
{
    self = [super initWithStyle:style];
    if (self) {
        // Custom initialization
    }
    return self;
}

-(void)setBackground {
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

- (void)viewDidLoad
{
    [super viewDidLoad];

    
    self.view.backgroundColor = [UIColor colorWithPatternImage:[UIImage imageNamed:@"wpbackground-notop.png"]];
    
    self.ebService = [EventBuddyService getInstance];
    [self refreshData];
}

-(void) refreshData {

    [self.ebService refreshSessionsForEvent:self.eventName OnSuccess:^{
        [self.tableView reloadData];
        [self performSelector:@selector(stopLoading)];
    }];
}

- (void)refresh {
    [self refreshData];
}

-(void) viewWillAppear:(BOOL)animated {
    [self setBackground];
}

//- (UIView *)tableView:(UITableView *)tableView viewForHeaderInSection:(NSInteger)section {
//    if (section == 5) {
//        CGRect headerViewRect = CGRectMake(0.0,0.0,320,40);
//        UIView* headerView = [[UIView alloc] initWithFrame:headerViewRect];
//        headerView.backgroundColor = [UIColor clearColor];
//        
//        //UIImageView *titleImage = [[UIImageView alloc] initWithImage:
//        //                            [UIImage imageNamed:@"wpbackground2.png"]];
//        //CGRect imageViewRect = CGRectMake(0.0,  0.0, 320 , 40);
//        //titleImage.frame = imageViewRect;
//        //titleImage.autoresizingMask = UIViewAutoresizingFlexibleLeftMargin;
//        //[headerView addSubview:titleImage];
//        
//        //Then you can add a UILabel to your headerView
//        return headerView;
//    }
//    return nil;
//}

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
    return [self.ebService.sessions count];
}

- (UITableViewCell *)tableView:(UITableView *)tableView cellForRowAtIndexPath:(NSIndexPath *)indexPath
{
    static NSString *CellIdentifier = @"Cell";
    UITableViewCell *cell = [tableView dequeueReusableCellWithIdentifier:CellIdentifier forIndexPath:indexPath];
    
    NSDictionary *session = [self.ebService.sessions objectAtIndex:indexPath.row];
    UILabel *sessionNameLabel = (UILabel *)[cell viewWithTag:1];
    sessionNameLabel.textColor = [UIColor blackColor];
    sessionNameLabel.text = [session objectForKey:@"name"];

    UILabel *sessionRoomLabel = (UILabel *)[cell viewWithTag:2];
    sessionRoomLabel.textColor = [UIColor blueColor];
    sessionRoomLabel.text = [session objectForKey:@"room"];
    
    UILabel *sessionSpeakerLabel = (UILabel *)[cell viewWithTag:3];
    sessionSpeakerLabel.textColor = [UIColor redColor];
    sessionSpeakerLabel.text = [session objectForKey:@"speaker"];
    
    NSDateFormatter* formatter = [[NSDateFormatter alloc] init];
    [formatter setDateFormat:@"MM.dd.yyyy"];
    
    UILabel *startLabel = (UILabel *)[cell viewWithTag:4];
    startLabel.textColor = [UIColor blueColor];
    NSDate *startDate = [session objectForKey:@"start"];
    startLabel.text = [NSString stringWithFormat:@"Starts:%@ | ", [formatter stringFromDate:startDate]];
    
    UILabel *endLabel = (UILabel *)[cell viewWithTag:5];
    endLabel.textColor = [UIColor blueColor];
    NSDate *endDate = [session objectForKey:@"end"];
    endLabel.text = [NSString stringWithFormat:@"Ends:%@", [formatter stringFromDate:endDate]];
    
    UIImageView *sessionImage = (UIImageView *)[cell viewWithTag:6];
    
    NSURL *url = [NSURL URLWithString:[session objectForKey:@"img"]];
    NSData *data = [NSData dataWithContentsOfURL:url];
    UIImage *image = [UIImage imageWithData:data];
    [sessionImage setImage:image];

    
    
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
    if ([segue.identifier isEqualToString:@"viewSessionDetails"]) {
        UITableViewCell *cell = (UITableViewCell *)sender;
        UILabel *sessionNameLabel = (UILabel *)[cell viewWithTag:1];
        SessionDetailsController *sessionDetailsController =
        segue.destinationViewController;
		sessionDetailsController.sessionName = sessionNameLabel.text;
        sessionDetailsController.eventName = self.eventName;
        
        
        for (id session in self.ebService.sessions) {
            NSString *name = [session objectForKey:@"name"];
            if ([name isEqualToString:sessionNameLabel.text]) {
                sessionDetailsController.session = session;
            }
        }
    } else if ([segue.identifier isEqualToString:@"newSessionSegue"]) {
        NewSessionViewController *newSessionViewController = segue.destinationViewController;
        newSessionViewController.delegate = self;
        newSessionViewController.eventId = self.eventId;
    }
}

@end
