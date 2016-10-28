#const width = 36.
#const height = 20.
xPos(0..width).
yPosLevel1(15..17).
yPosLevel2(13..15).
yPosLevel3(11..13).
yPosLevel4(9..11).
yPosLevel5(7..9).
yPosLevel6(5..7).
yPosLevel7(3..5).
tileType(0..2).

20 { tile(X, Y, 1) : xPos(X), yPosLevel1(Y) }.

20 { tile(X, Y, 1) : xPos(X), yPosLevel2(Y) }.

20 { tile(X, Y, 1) : xPos(X), yPosLevel3(Y) }.

20 { tile(X, Y, 1) : xPos(X), yPosLevel4(Y) }.

10 { tile(X, Y, 1) : xPos(X), yPosLevel5(Y) } 10.

10 { tile(X, Y, 1) : xPos(X), yPosLevel6(Y) } 10.

20 { tile(X, Y, 1) : xPos(X), yPosLevel7(Y) }.

strip(tile(X, Y, 1), tile(XX, YY, 1)) :- tile(X, Y, 1), tile(XX, YY, 1), |(X-XX)|+|Y-YY| == 0.

%creating the ground
tile(0..width, 19..height, 0).

%creating the sky
tile(0..width, 0..height, 2).

%constraining output to only print the tiles
#show tile/3.
















%setting a constraint that no two tiles with tileType 1 can have the same xPos
%:- tile(X,Y,1), tile(XX,Y,1), X != XX.



%creating 20 tiles with random x and y positions with tileType 1
%20 { tile(X, Y, 1) : xPos(X), yPos(Y) }.


%tile(X, Y, T) :- xPos(X), yPos(Y), tileType(T).

%strip(tile(X, Y, 1), tile(XX, YY, 1)) :- tile(X, Y, 1), tile(XX, YY, 1), |(X-XX)|+|Y-YY| == 1.

%pyramid(tile(X, Y, 1), tile(XX, YY, 1)) :- tile(X, Y, 1), tile(XX, YY, 1), |(X-XX)|+|Y-YY| == 5.

%(width*height)/5 { tile(X, Y, 1) : xPos(X), yPos(Y) } .

%10 {strip(tile(X, Y, 1), tile(XX, YY, 1)): xPos(X), xPos(XX), yPos(Y), yPos(YY)} 10.
