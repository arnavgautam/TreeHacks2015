//
//  SessionDetailsController.m
//  EventBuddy
//
//  Created by Chris Risner on 11/4/12.
//  Copyright (c) 2012 Microsoft. All rights reserved.
//

#import "SessionDetailsController.h"
#import "EventBuddyService.h"

@interface SessionDetailsController ()

@end

@implementation SessionDetailsController

- (id)initWithNibName:(NSString *)nibNameOrNil bundle:(NSBundle *)nibBundleOrNil
{
    self = [super initWithNibName:nibNameOrNil bundle:nibBundleOrNil];
    if (self) {
        // Custom initialization
    }
    return self;
}

-(void)setBackground{
    //Resize background
    UIGraphicsBeginImageContext(self.view.frame.size);
    [[UIImage imageNamed:@"wpbackground2.png"] drawInRect:self.view.bounds];
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

- (void)viewDidLoad
{
    [super viewDidLoad];
    
    //[self setBackground];
        
    
	// Do any additional setup after loading the view.
    self.lblSessionName.text = [self.session objectForKey:@"name"];
    self.lblSpeakerName.text = [NSString stringWithFormat:@"@%@", [self.session objectForKey:@"speaker"]];
    self.lblDescription.text = [self.session objectForKey:@"description"];
        
    NSURL *url = [NSURL URLWithString:[self.session objectForKey:@"img"]];
    NSData *data = [NSData dataWithContentsOfURL:url];
    UIImage *image = [UIImage imageWithData:data];
    [self.imgSession setImage:image];

    NSDateFormatter* formatter = [[NSDateFormatter alloc] init];
    [formatter setDateFormat:@"EEEE, MMM d, yyyy"];
    NSDate *startDate = [self.session objectForKey:@"start"];
    self.lblDate.text = [formatter stringFromDate:startDate];

    [formatter setDateFormat:@"h:mm"];
    [formatter setTimeZone:[NSTimeZone timeZoneWithAbbreviation:@"GMT"]];
    NSDate *endDate = [self.session objectForKey:@"end"];
    self.lblTime.text = [NSString stringWithFormat:@"%@ - %@",
                         [formatter stringFromDate:startDate],
                         [formatter stringFromDate:endDate]];
}

- (void)didReceiveMemoryWarning
{
    [super didReceiveMemoryWarning];
    // Dispose of any resources that can be recreated.
}

- (IBAction)tappedStarOne:(id)sender {
    [self setRatingWithStars:1];
    [self saveRating:1];
}

- (IBAction)tappedStarTwo:(id)sender {
    [self setRatingWithStars:2];
    [self saveRating:2];
}

- (IBAction)tappedStarThree:(id)sender {
    [self setRatingWithStars:3];
    [self saveRating:3];
}

- (IBAction)tappeStarFour:(id)sender {
    [self setRatingWithStars:4];
    [self saveRating:4];
}

- (IBAction)tappedStarFive:(id)sender {
    [self setRatingWithStars:5];
    [self saveRating:5];
}

-(void) saveRating:(int)rating {
    
    
    EventBuddyService *ebService = [EventBuddyService getInstance];
    //Get the twitter ID
    NSString *twitterId = [ebService.client.currentUser.userId stringByReplacingOccurrencesOfString:@"Twitter:" withString:@""];
    //Request information on the current user using their twitter ID
    NSString *queryString = [NSString stringWithFormat:@"https://api.twitter.com/users/%@.json", twitterId];
    NSURLRequest *theRequest=[NSURLRequest
                              requestWithURL:[NSURL URLWithString:
                                              queryString]
                              cachePolicy:NSURLRequestUseProtocolCachePolicy
                              timeoutInterval:60.0];
    [NSURLConnection sendAsynchronousRequest:theRequest queue:[NSOperationQueue mainQueue] completionHandler:^(NSURLResponse *response, NSData *data, NSError *error) {
        NSString *imageUrl = @"https://twimg0-a.akamaihd.net/profile_images/1349731834/profile2_reasonably_small.png";
        NSString *raterName = @"Nick Harris";
        if (error) {
            //do something with error
            NSLog(@"There was an error! %@", error);
        } else {
            //get the Profile Image URL from the twitter data
            NSString *responseText = [[NSString alloc] initWithData:data encoding: NSASCIIStringEncoding];
            NSDictionary* json = [NSJSONSerialization
                             JSONObjectWithData:data
                             options:kNilOptions
                             error:&error];
            imageUrl = [json objectForKey:@"profile_image_url"];
            raterName = [json objectForKey:@"name"];
        }
        
        //Create the rating and save it
        NSDictionary *ratingItem = @{ @"sessionId" : [self.session objectForKey:@"id"]
        , @"rating" : [NSNumber numberWithInt:rating]
        , @"imageUrl" : imageUrl
        , @"raterName" : raterName};
        
        [ebService saveRating:ratingItem completion:^(NSUInteger index) {
            NSLog(@"Saved rating");
        }];
    }];
    
    
    
    
}

-(void) setRatingWithStars:(int)starCount {
    UIImage *selectedStarImage = [UIImage imageNamed:@"SelectedStar.png"];
    UIImage *unselectedStarImage = [UIImage imageNamed:@"UnselectedStar.png"];
    
    if (starCount == 1) {
        [self.btnStarOne setBackgroundImage:selectedStarImage forState: UIControlStateSelected & UIControlStateApplication];
        [self.btnStarTwo setBackgroundImage:unselectedStarImage forState:UIControlStateSelected & UIControlStateApplication];
        [self.btnStarThree setBackgroundImage:unselectedStarImage forState:UIControlStateSelected & UIControlStateApplication];
        [self.btnStarFour setBackgroundImage:unselectedStarImage forState:UIControlStateSelected & UIControlStateApplication];
        [self.btnStarFive setBackgroundImage:unselectedStarImage forState:UIControlStateSelected & UIControlStateApplication];
    } else if (starCount == 2) {
        [self.btnStarOne setBackgroundImage:selectedStarImage forState:UIControlStateSelected & UIControlStateApplication];
        [self.btnStarTwo setBackgroundImage:selectedStarImage forState:UIControlStateSelected & UIControlStateApplication];
        [self.btnStarThree setBackgroundImage:unselectedStarImage forState:UIControlStateSelected & UIControlStateApplication];
        [self.btnStarFour setBackgroundImage:unselectedStarImage forState:UIControlStateSelected & UIControlStateApplication];
        [self.btnStarFive setBackgroundImage:unselectedStarImage forState:UIControlStateSelected & UIControlStateApplication];
    } else if (starCount == 3) {
        [self.btnStarOne setBackgroundImage:selectedStarImage forState:UIControlStateSelected & UIControlStateApplication];
        [self.btnStarTwo setBackgroundImage:selectedStarImage forState:UIControlStateSelected & UIControlStateApplication];
        [self.btnStarThree setBackgroundImage:selectedStarImage forState:UIControlStateSelected & UIControlStateApplication];
        [self.btnStarFour setBackgroundImage:unselectedStarImage forState:UIControlStateSelected & UIControlStateApplication];
        [self.btnStarFive setBackgroundImage:unselectedStarImage forState:UIControlStateSelected & UIControlStateApplication];
    } else if (starCount == 4) {
        [self.btnStarOne setBackgroundImage:selectedStarImage forState:UIControlStateSelected & UIControlStateApplication];
        [self.btnStarTwo setBackgroundImage:selectedStarImage forState:UIControlStateSelected & UIControlStateApplication];
        [self.btnStarThree setBackgroundImage:selectedStarImage forState:UIControlStateSelected & UIControlStateApplication];
        [self.btnStarFour setBackgroundImage:selectedStarImage forState:UIControlStateSelected & UIControlStateApplication];
        [self.btnStarFive setBackgroundImage:unselectedStarImage forState:UIControlStateSelected & UIControlStateApplication];
    } else if (starCount == 5) {
        [self.btnStarOne setBackgroundImage:selectedStarImage forState:UIControlStateSelected & UIControlStateApplication];
        [self.btnStarTwo setBackgroundImage:selectedStarImage forState:UIControlStateSelected & UIControlStateApplication];
        [self.btnStarThree setBackgroundImage:selectedStarImage forState:UIControlStateSelected & UIControlStateApplication];
        [self.btnStarFour setBackgroundImage:selectedStarImage forState:UIControlStateSelected & UIControlStateApplication];
        [self.btnStarFive setBackgroundImage:selectedStarImage forState:UIControlStateSelected & UIControlStateApplication];
    }
}


- (IBAction)tappedViewDocument:(id)sender {
    NSURL *url = [ [ NSURL alloc ] initWithString: [self.session objectForKey:@"deckSource"]];
    [[UIApplication sharedApplication] openURL:url];
}
@end
