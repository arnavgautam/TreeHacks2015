//
//  EventBuddyService.h
//  EventBuddy
//
//  Created by Chris Risner on 11/4/12.
//  Copyright (c) 2012 Microsoft. All rights reserved.
//

#import <Foundation/Foundation.h>
#import <WindowsAzureMobileServices/WindowsAzureMobileServices.h>

#pragma mark * Block Definitions


typedef void (^CompletionBlock) ();
typedef void (^CompletionWithIndexBlock) (NSUInteger index);
typedef void (^BusyUpdateBlock) (BOOL busy);



@interface EventBuddyService : NSObject<MSFilter>

@property (nonatomic, strong)   NSArray *events;
@property (nonatomic, strong)   NSArray *ratings;
@property (nonatomic, strong)   NSArray *sessions;
@property (nonatomic, strong)   MSClient *client;
@property (nonatomic, copy)     BusyUpdateBlock busyUpdate;


+(EventBuddyService*) getInstance;

- (void) refreshEventsOnSuccess:(CompletionBlock) completion;
- (void) refreshSessionsForEvent:(NSString *)eventName OnSuccess:(CompletionBlock) completion;
- (void) refreshRatingsOnSuccess:(CompletionBlock) completion;
- (void) saveRating:(NSDictionary *)rating
         completion:(CompletionWithIndexBlock) completion;
- (void) saveEvent:(NSDictionary *)event
         completion:(CompletionWithIndexBlock) completion;
- (void) saveSession:(NSDictionary *)session
         completion:(CompletionWithIndexBlock) completion;
- (void) handleRequest:(NSURLRequest *)request
                onNext:(MSFilterNextBlock)onNext
            onResponse:(MSFilterResponseBlock)onResponse;
@end
