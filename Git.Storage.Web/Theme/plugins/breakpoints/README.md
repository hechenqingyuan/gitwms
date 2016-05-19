# Breakpoints.js

Define breakpoints for your responsive design, and Breakpoints.js will fire custom events when the browser enters and/or exits that breakpoint.

[Get it from Github](https://github.com/xoxco/breakpoints)

[View Demo](http://xoxco.com/projects/code/breakpoints/)

Created by [XOXCO](http://xoxco.com)

## Instructions

	$(window).setBreakpoints({
	// use only largest available vs use all available
		distinct: true, 
	// array of widths in pixels where breakpoints
	// should be triggered
		breakpoints: [
			320,
			480,
			768,
			1024
		] 
	});		
	
	$(window).bind('enterBreakpoint320',function() {
		...
	});
	
	$(window).bind('exitBreakpoint320',function() {
		...
	});
	
	$(window).bind('enterBreakpoint768',function() {
		...
	});
	
	$(window).bind('exitBreakpoint768',function() {
		...
	});
	
	
	$(window).bind('enterBreakpoint1024',function() {
		...
	});
	
	$(window).bind('exitBreakpoint1024',function() {
		...
	});

