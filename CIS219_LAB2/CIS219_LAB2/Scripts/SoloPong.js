
//
// SoloPong is a HTML5/CSS3 JavaScript game based on
// the ping-pong tutorial by Kushagra Agarwal available at:
// http://cssdeck.com/labs/ping-pong-game-tutorial-with-html5-canvas-and-sounds
//
// I've turned the board side-ways and changed the end-game to submit a form, for ASP.NET's use
// Author: Amelia Garripoli
// Date: 26 Sep 2015
// License: CC-BY-NC-4.0 https://creativecommons.org/licenses/by-nc/4.0/
//

window.requestAnimFrame = (function () {
    return window.requestAnimationFrame ||
        window.webkitRequestAnimationFrame ||
        window.mozRequestAnimationFrame ||
        window.oRequestAnimationFrame ||
        window.msRequestAnimationFrame ||
        function (callback) {
            return window.setTimeout(callback, 1000 / 60);
        };
})();

window.cancelRequestAnimFrame = (function () {
    return window.cancelAnimationFrame ||
        window.webkitCancelRequestAnimationFrame ||
        window.mozCancelRequestAnimationFrame ||
        window.oCancelRequestAnimationFrame ||
        window.msCancelRequestAnimationFrame ||
        clearTimeout
})();


// Initialize canvas and required variables
var canvas = document.getElementById("soloPong"),
        ctx = canvas.getContext("2d"), // Create canvas context
        W = canvas.width, // Window's width -- set by canvas
        H = canvas.height, // Window's height -- set by canvas
        R = 10, // radius of the ball
        PW = 10, // thickness of the paddle
        PH = 100, // height of the paddle
        SP = 5, // how often to speed the ball
        SH = 10, // how often to shrink the paddle (shrinks by RADIUS down to RADIUS, stops there)
        V = 3, // velocity constant, to start
        DV = 1, // change in velocity
        LEFT = 1, // left paddle id
        RIGHT = 2, // right paddle id
        particles = [], // Array containing particles
        ball = {}, // Ball object
        paddles = [], // Array containing two paddles
        mouse = {}, // Mouse object to store its current position
        points = 0, // Varialbe to store points
        fps = 60, // Max FPS (frames per second)
        particlesCount = 20, // Number of sparks when ball strikes the paddle
        flag = 0, // Flag variable which is changed on collision
        particlePos = {}, // Object to contain the position of collision
        multipler = 1, // Variable to control the direction of sparks
        startBtn = {}, // Start button object
        over = 0, // flag variable, changed when the game is over
        submit = 1, // flag variable, changed once submit underway
        init, // variable to initialize animation
        paddleHit;

// Add mousemove and mousedown events to the canvas
canvas.addEventListener("mousemove", trackPosition, true);
canvas.addEventListener("mousedown", btnClick, true);

// Initialize the collision sound
collision = document.getElementById("collide");

// Function to paint canvas
function paintCanvas() {
    ctx.fillStyle = "black";
    ctx.fillRect(0, 0, W, H);
}

// Function for creating paddles
function Paddle(pos) {
    // Height and width
    this.h = PH;
    this.w = PW;

    // Paddle's position
    this.x = (pos == LEFT) ? 0 : W - this.w;
    this.y = H / 2 - this.h / 2;

}

// Push two new paddles into the paddles[] array
paddles.push(new Paddle(LEFT));
paddles.push(new Paddle(RIGHT));

// Ball object
ball = {
    x: 50,
    y: 50,
    r: R, // radius
    c: "white", // color
    vx: V, // velocity
    vy: V,

    // Function for drawing ball on canvas
    draw: function () {
        ctx.beginPath();
        ctx.fillStyle = this.c;
        ctx.arc(this.x, this.y, this.r, 0, Math.PI * 2, false);
        ctx.fill();
    }
};


// Start Button object
startBtn = {
    w: 100,
    h: 50,
    x: W / 2 - 50,
    y: H / 2 - 25,

    draw: function () {
        ctx.strokeStyle = "white";
        ctx.lineWidth = "2";
        ctx.strokeRect(this.x, this.y, this.w, this.h);

        ctx.font = "18px Arial, sans-serif";
        ctx.textAlign = "center";
        ctx.textBaseline = "middle";
        ctx.fillStlye = "white";
        ctx.fillText("Start", W / 2, H / 2);
    }
};

// Function for creating particles object
function createParticles(x, y, m) {
    this.x = x || 0;
    this.y = y || 0;

    this.radius = 2.5;

    this.vx = -1.5 + Math.random() * 3;
    this.vy = m * Math.random() * 1.5;
}

// Draw everything on canvas
function draw() {
    paintCanvas();
    for (var i = 0; i < paddles.length; i++) {
        p = paddles[i];

        ctx.fillStyle = "white";
        ctx.fillRect(p.x, p.y, p.w, p.h);
    }

    ball.draw();
    update();
}

// Function to increase speed after every 5 points
// alternative: shrink the paddles!
function increaseSpd() {
    if (points % SP == 0) {
        if (Math.abs(ball.vx) < 15) {
            ball.vx += (ball.vx < 0) ? -DV : DV; // was twice vy's change
            ball.vy += (ball.vy < 0) ? -DV : DV;
        }
    }
}

// Function to shrink the paddles after every 10 points
function shrinkPdl() {
    if (points % SH == 0) {
        for (var i = 0; i < paddles.length; i++) {
            p = paddles[i];
            if (p.h > R) {
                p.h = p.h - R;

                // Paddle's position (recenter on new height)
                p.y = H / 2 - p.h / 2;

            }
        }
    }
}

// Track the position of mouse cursor
function trackPosition(e) {
    mouse.x = e.pageX;
    mouse.y = e.pageY;
}

// Function to update positions, score and everything.
// Basically, the main game logic is defined here
function update() {

    // Update scores
    updateScore();

    // Move the paddles on mouse move
    if (mouse.x && mouse.y) {
        for (var i = 0; i < paddles.length; i++) {
            p = paddles[i];
            p.y = mouse.y - p.h / 2;
        }
    }

    // Move the ball
    ball.x += ball.vx;
    ball.y += ball.vy;

    // Collision with paddles
    p1 = paddles[0];
    p2 = paddles[1];

    // If the ball strikes with paddles,
    // invert the y-velocity vector of ball,
    // increment the points, play the collision sound,
    // save collision's position so that sparks can be
    // emitted from that position, set the flag variable,
    // and change the multiplier
    if (collides(ball, p1)) {
        collideAction(ball, p1);
    }
    else if (collides(ball, p2)) {
        collideAction(ball, p2);
    }
    else {
        // Collide with walls, If the ball hits the left/right,
        // walls, run gameOver() function
        if (ball.x > W) {
            ball.x = W - ball.r;
            gameOver();
        }
        else if (ball.x < 0) {
            ball.x = ball.r;
            gameOver();
        }

        // If ball strikes the horizontal walls, invert the
        // y-velocity vector of ball
        if (ball.y + ball.r > H) {
            ball.vy = -ball.vy;
            ball.y = H - ball.r;
        }
        else if (ball.y - ball.r < 0) {
            ball.vy = -ball.vy;
            ball.y = ball.r;
        }
    }

    // If flag is set, push the particles
    if (flag == 1) {
        for (var k = 0; k < particlesCount; k++) {
            particles.push(new createParticles(particlePos.x, particlePos.y, multiplier));
        }
    }

    // Emit particles/sparks
    emitParticles();

    // reset flag
    flag = 0;
}

//Function to check collision between ball and one of
//the paddles
function collides(b, p) {

    if (b.y + ball.r >= p.y && b.y - ball.r <= p.y + p.h) {
        if (b.x >= (p.x - p.w) && p.x > 0) {
            paddleHit = RIGHT;
            return true;
        }

        else if (b.x <= p.w && p.x == 0) {
            paddleHit = LEFT;
            return true;
        }

        else return false;

    }
}

//Do this when collides == true
function collideAction(ball, p) {
    ball.vx = -ball.vx;

    if (paddleHit == LEFT) {
        ball.x = p.x + p.w;
        particlePos.x = ball.x + ball.r;
        multiplier = 1;
    }

    else if (paddleHit == RIGHT) {
        ball.x = p.x - p.w - ball.r;
        particlePos.x = ball.x - ball.r;
        multiplier = -1;
    }

    points++;
    increaseSpd();
    // shrinkPdl();

    if (collision) {
        if (points > 0)
            collision.pause();

        collision.currentTime = 0;
        //collision.play();
    }

    particlePos.x = ball.x;
    flag = 1;
}

// Function for emitting particles
function emitParticles() {
    for (var j = 0; j < particles.length; j++) {
        par = particles[j];

        ctx.beginPath();
        ctx.fillStyle = "white";
        if (par.radius > 0) {
            ctx.arc(par.x, par.y, par.radius, 0, Math.PI * 2, false);
        }
        ctx.fill();

        par.x += par.vx;
        par.y += par.vy;

        // Reduce radius so that the particles die after a few seconds
        par.radius = Math.max(par.radius - 0.05, 0.0);

    }
}

// Function for updating score
function updateScore() {
    ctx.fillStlye = "white";
    ctx.font = "16px Arial, sans-serif";
    ctx.textAlign = "left";
    ctx.textBaseline = "top";
    ctx.fillText("Score: " + points, 20, 20);
}

// Function to run when the game overs
function gameOver() {
    ctx.font = "20px Arial, sans-serif";
    ctx.textAlign = "center";
    ctx.textBaseline = "middle";
    ctx.fillStlye = "white";
    ctx.fillText("Click to Continue", W / 2, H / 2 - 25);

    ctx.fillText("Game Over - You scored " + points + " points!", W / 2, H / 2 + 25);

    // Stop the Animation
    cancelRequestAnimFrame(init);

    // Set the over flag
    over = 1;
}

// Function for running the whole animation
function animloop() {
    init = requestAnimFrame(animloop);
    draw();
}

// Function to execute at startup
function startScreen() {
    draw();
    startBtn.draw();
}

// On button click (Restart and start)
function btnClick(e) {

    // Variables for storing mouse position on click
    var mx = e.pageX,
            my = e.pageY;
    // If the game is over, reset
    if (over == 1) {
        submit = 0;
        over = 0;
        // only post it once -- ignores additional clicks
        // post score and submit form
        // NOTE: if you changed the form fields in the view, change the fields set here to match
        document.forms["gameOver"].Score.value = points;
        document.forms["gameOver"].timeStamp.value = Date();
        document.gameOver.submit();
    } else     // Click start button if submit not underway already
    if (submit == 1)
    {
        animloop();
        // Delete the start button after clicking it
        startBtn = {};
    }

}
