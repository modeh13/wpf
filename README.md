# WPF
This contains some WPF applications developed with .Net Framework as learning.

- **WpfDrawingTool:**
It's a small application developed for the selection process with HUGE Inc. It's code challenge, basically, the tool receives some commands line to draw on Canvas (Text file) with a few conditions, below, there're the main commands.

- C w h Create Canvas: Should create a new canvas of width w and height h.
- L x1 y1 x2 y2 Create Line: Should create a new line from (x1,y1) to (x2,y2). Currently only horizontal or vertical lines are supported. Horizontal and vertical lines will be drawn using the 'x' character.
- R x1 y1 x2 y2 Create Rectangle: Should create a new rectangle, whose upper left corner is (x1,y1) and lower right corner is (x2,y2). Horizontal and vertical lines will be drawn using the 'x' character.
- B x y c Bucket Fill: Should fill the entire area connected to (x,y) with "colour" c. The behaviour of this is the same as that of the "bucket fill" tool in paint programs.

For example:
