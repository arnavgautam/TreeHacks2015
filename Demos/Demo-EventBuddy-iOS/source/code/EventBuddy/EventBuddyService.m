//
//  EventBuddyService.m
//  EventBuddy
//
//  Created by Chris Risner on 11/4/12.
//  Copyright (c) 2012 Microsoft. All rights reserved.
//

#import "EventBuddyService.h"
#import <WindowsAzureMobileServices/WindowsAzureMobileServices.h>

@interface EventBuddyService()

@property (nonatomic, strong)   MSTable *eventsTable;
@property (nonatomic, strong)   MSTable *sessionsTable;
@property (nonatomic, strong)   MSTable *ratingsTable;
@property (nonatomic)           NSInteger busyCount;

@end

@implementation EventBuddyService

static EventBuddyService *singletonInstance;

+ (EventBuddyService*)getInstance{
    if (singletonInstance == nil) {
        singletonInstance = [[super alloc] init];
    }
    return singletonInstance;
}

-(EventBuddyService *) init
{
    // Initialize the Mobile Service client with your URL and key
    MSClient *newClient = [MSClient clientWithApplicationURLString:@"https://{mobile-service-name}.azure-mobile.net/"
        withApplicationKey:@"{mobile-service-key}"];
    
    // Add a Mobile Service filter to enable the busy indicator
    self.client = [newClient clientwithFilter:self];
    
    // Create an MSTable instance to allow us to work with the TodoItem table
    //self.table = [_client getTable:@"TodoItem"];
    self.eventsTable = [_client getTable:@"Event"];
    self.sessionsTable = [_client getTable:@"Session"];
    self.ratingsTable = [_client getTable:@"Rating"];
    
    self.events = [[NSMutableArray alloc] init];
    self.ratings = [[NSMutableArray alloc] init];
    self.sessions = [[NSMutableArray alloc] init];
    self.busyCount = 0;
    
    return self;
}

- (void) refreshEventsOnSuccess:(CompletionBlock)completion
{
    [self.eventsTable readWithCompletion:^(NSArray *results, NSInteger totalCount, NSError *error) {
        
        [self logErrorIfNotNil:error];
        
        self.events = [results mutableCopy];
        
        // Let the caller know that we finished
        completion();
    }];
}

- (void) refreshSessionsForEvent:(NSString *)eventName OnSuccess:(CompletionBlock) completion
{
    int eventId = 0;
    for (id event in self.events) {
        NSString *name = [event objectForKey:@"name"];
        if ([name isEqualToString:eventName]) {
            eventId = [(NSNumber *)[event objectForKey:@"id"] intValue];
        }
    }
    
    // Create a predicate that finds items where complete is false
    NSPredicate * predicate = [NSPredicate predicateWithFormat:@"eventId == %i", eventId];
    
    // Query the TodoItem table and update the items property with the results from the service
    [self.sessionsTable readWhere:predicate completion:^(NSArray *results, NSInteger totalCount, NSError *error) {
        
        [self logErrorIfNotNil:error];
        
        self.sessions = [results mutableCopy];
        
        // Let the caller know that we finished
        completion();
    }];
}

- (void) refreshRatingsOnSuccess:(CompletionBlock)completion
{
    [self.ratingsTable readWithCompletion:^(NSArray *results, NSInteger totalCount, NSError *error) {
        
        [self logErrorIfNotNil:error];
        
        self.ratings = [results mutableCopy];
        
        // Let the caller know that we finished
        completion();
    }];
}

- (void) saveRating:(NSDictionary *)rating
         completion:(CompletionWithIndexBlock) completion {
    [self.ratingsTable insert:rating completion:^(NSDictionary *result, NSError *error) {
        
        [self logErrorIfNotNil:error];
        
        NSUInteger index = [self.ratings count];
        [(NSMutableArray *)self.ratings insertObject:result atIndex:index];
        
        // Let the caller know that we finished
        completion(index);
    }];
}

- (void) saveEvent:(NSDictionary *)event
         completion:(CompletionWithIndexBlock) completion {
    [self.eventsTable insert:event completion:^(NSDictionary *result, NSError *error) {
        
        [self logErrorIfNotNil:error];
        
        NSUInteger index = [self.events count];
        [(NSMutableArray *)self.events insertObject:result atIndex:index];
        
        // Let the caller know that we finished
        completion(index);
    }];
}

- (void) saveSession:(NSDictionary *)session
         completion:(CompletionWithIndexBlock) completion {
    [self.sessionsTable insert:session completion:^(NSDictionary *result, NSError *error) {
        
        [self logErrorIfNotNil:error];
        
        NSUInteger index = [self.sessions count];
        [(NSMutableArray *)self.sessions insertObject:result atIndex:index];
        
        // Let the caller know that we finished
        completion(index);
    }];
}


- (void) busy:(BOOL) busy
{
    // assumes always executes on UI thread
    if (busy) {
        if (self.busyCount == 0 && self.busyUpdate != nil) {
            self.busyUpdate(YES);
        }
        self.busyCount ++;
    }
    else
    {
        if (self.busyCount == 1 && self.busyUpdate != nil) {
            self.busyUpdate(FALSE);
        }
        self.busyCount--;
    }
}

- (void) logErrorIfNotNil:(NSError *) error
{
    if (error) {
        NSLog(@"ERROR %@", error);
    }
}



#pragma mark * MSFilter methods


- (void) handleRequest:(NSURLRequest *)request
                onNext:(MSFilterNextBlock)onNext
            onResponse:(MSFilterResponseBlock)onResponse
{
    // A wrapped response block that decrements the busy counter
    MSFilterResponseBlock wrappedResponse = ^(NSHTTPURLResponse *response, NSData *data, NSError *error) {
        [self busy:NO];
        onResponse(response, data, error);
    };
    
    // Increment the busy counter before sending the request
    [self busy:YES];
    onNext(request, wrappedResponse);
}

@end
