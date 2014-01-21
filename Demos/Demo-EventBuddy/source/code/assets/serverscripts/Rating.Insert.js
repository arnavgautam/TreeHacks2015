function insert(item, user, request) {
    request.execute({
        success: function() {
            request.respond();
            sendNotifications(item, user);
        }
    });
}

function sendNotifications(item, user) {
    var sql = "SELECT DISTINCT c.uri, s.name " + 
              "FROM Channel c INNER JOIN Session s " + 
                "ON c.userId = s.userId AND s.id = ?";
                
    mssql.query(sql, [item.sessionId], {
        success: function(results) {
            if (results.length === 0) return;
            var channels = results.map(function(r) { return r.uri });
            
            // Send the push notification
            push.wns.sendTileWideSmallImageAndText04(channels, {
                image1src: item.imageUrl,
                text1: item.rating + " stars",
                text2: item.raterName + " just rated your session '" +
                         results[0].name + "'"
            });
        }
    });
}