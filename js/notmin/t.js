s.url = ( ( url || s.url || location.href ) + "" )
.replace( rprotocol, location.protocol + "//" );

// Alias method option to type as per ticket #12004
s.type = options.method || options.type || s.method || s.type;