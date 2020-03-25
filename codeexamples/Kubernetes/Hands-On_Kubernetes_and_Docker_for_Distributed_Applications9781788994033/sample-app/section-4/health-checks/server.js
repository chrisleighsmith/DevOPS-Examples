var restify = require('restify');

var server = restify.createServer();
var healthy = true;

setTimeout(() =>{
    healthy = false;
}, 30000);

server.get('/healthz', (req,res,next)=>{
  if(!healthy){
    return next(new Error("UNHEALTHY"));
  }
  res.send('OK');
  next();
});


server.listen(8080, function() {
  console.log('%s listening at %s', server.name, server.url);
});

var ready = false;

setTimeout(()=>{
    ready = true;
}, 15000);

server.get('/ready', (req,res,next)=>{
  if(!ready){
    return next(new Error("NOT READY"));
  }
  res.send('OK');
  next();
});
