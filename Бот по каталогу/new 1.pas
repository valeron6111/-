var x: real; 
begin 
if (x<1.2) then 
y:=2.8*sqr(x)-0.3*x+4; 
else 
if (x=1.2)then 
y:=(2.8/x)+sqrt(sqr(x)+1);
else 
if (x>1.2) then 
y:=(2.8-0.3*x)/(sqrt(sqr(x)+1)); 
writeln('Y = ',y:3); 
end. 