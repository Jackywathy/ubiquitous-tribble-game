//
//  player.swift
//  My Game
//
//  Created by Vincent Liu on 27/7/17.
//  Copyright © 2017 Vincent Liu. All rights reserved.
//

import Foundation
import SpriteKit

let Fgravity = 0.7 // should not be bigger than Player.moveSpeed.y
let Ffriction = 1.0 // should not be bigger than Player.moveSpeed.x
let FairResist = 2.5 // should not be bigger than Player.moveSpeed.x

// NOTE: Friction and air resistance should not decelerate objects below 0, so use decreaseMagnitude(), not accelerateX()

struct Velocity {
	// In pixels per frame @ 60fps
	var x: Double
	var y: Double
}

class Player {
	
	var sprite: SKSpriteNode
	
	let moveSpeed = Velocity(x: 3.0, y: 20.0)
	var veloc = Velocity(x: 0.0, y: 0.0)
	private let maxVeloc = Velocity(x: 8.0, y: -9.0) // No maxVeloc for positive y as it can't be constantly increased
	
	var isGrounded = true
	var isJumping = false
	
	private var lastGroundObject: SKSpriteNode?
	
	func doCollisionCheck(_ block: SKSpriteNode) {
		
		let selfCentre = self.sprite.position
		let selfRightmost = self.sprite.frame.maxX
		let selfLeftmost = self.sprite.frame.minX
		let selfUppermost = self.sprite.frame.maxY
		let selfLowermost = self.sprite.frame.minY
		
		let blockRightmost = block.frame.maxX
		let blockLeftmost = block.frame.minX
		let blockUppermost = block.frame.maxY
		let blockLowermost = block.frame.minY
		
		let insideFromLeft = selfRightmost > blockLeftmost
		let insideFromRight = selfLeftmost < blockRightmost
		let insideFromBelow = selfUppermost > blockLowermost
		let insideFromAbove = selfLowermost < blockUppermost
		
		
		/* "Cross" implementation for collision
		
		     |N  |
		   __|   |__
		   W  XXX  E
		   __ XXX __
		     |   |
		     |S  |
		
		*/
		
		// NOTE: N and S have priority on purpose
		
		/*
			*---*
			| N |
			*---*
		*/
		
		// minor adjustment to selfLowermost so that jumping on blocks from below works better
		// still has issues with snapping to ground and positioning
		
		if (selfLowermost + (0.1 * self.sprite.size.height)) > blockUppermost && insideFromAbove && insideFromLeft && insideFromRight {
			
			isGrounded = true
			lastGroundObject = block
			self.sprite.position.y = blockUppermost + (0.5 * self.sprite.size.height)
			self.veloc.y = 0
			
		}
			
		/*
			*---*
			| S |
			*---*
		*/
			
		// "self.veloc.y > 0" - If player is falling, they should not collide with an object from below anyway
			
		// "(selfUppermost + (self.sprite.size.height * 0.05)) > blockLowermost" - modification of insideFromBelow with small adjustment to trigger earlier (accounting for object's high vertical speed)
			
		else if self.veloc.y > 0 && selfCentre.y < blockLowermost && (selfUppermost + (self.sprite.size.height * 0.05)) > blockLowermost && insideFromLeft && insideFromRight {
			
			// needs to be 0.7x here for some reason, otherwise it puts player inside block
			self.sprite.position.y = blockLowermost - (0.7 * self.sprite.size.height)
			self.veloc.y = 0
			
		}
		
		/*
			*---*
			| W |
			*---*
		*/
		
		else if selfCentre.x < blockLeftmost && insideFromLeft && insideFromAbove && insideFromBelow {
			self.sprite.position.x = blockLeftmost - (0.5 * self.sprite.size.width)
			self.veloc.x = 0
			
		}
		
		/*
			*---*
			| E |
			*---*
		*/
		
		else if selfCentre.x > blockRightmost && insideFromRight && insideFromAbove && insideFromBelow {
			self.sprite.position.x = blockRightmost + (0.5 * self.sprite.size.width)
			self.veloc.x = 0
			
		}
			
		// Check for falling off of a platform
		// if this block is what player was standing on last, and player is no longer on this block
			
		else if block == lastGroundObject && !(insideFromLeft && insideFromRight) {
			isGrounded = false
		}
		
		// Quadrant implementation
		// (just in case)
		/* Collision analysis
		
				  ^
			2	  |		1
				XX|XX
		   <------+------>
				XX|XX
			3	  |		4
			      v
		
			Divide block into 4 quadrants
		
		
		
		// NOTE: If pos is exactly on lines, it should be in upper/right quadrant
		
		let inFirstQuadrant = selfCenter.x >= blockCenter.x && selfCenter.y >= blockCenter.y ? true : false
		let inSecondQuadrant = selfCenter.x < blockCenter.x && selfCenter.y >= blockCenter.y ? true : false
		let inThirdQuadrant = selfCenter.x < blockCenter.x && selfCenter.y < blockCenter.y ? true : false
		let inFourthQuadrant = selfCenter.x >= blockCenter.x && selfCenter.y < blockCenter.y ? true : false
		
		// now actually check for collision
		if insideFromAbove && ((inFirstQuadrant && insideFromRight) || (inSecondQuadrant && insideFromLeft)) {
			self.sprite.position.y = blockUppermost + (0.5 * self.sprite.size.height)
			self.veloc.y = 0
			isGrounded = true
			lastGroundObject = block
	
		} else if insideFromBelow && ((inFourthQuadrant && insideFromRight) || (inThirdQuadrant && insideFromLeft)) {
			self.sprite.position.y = blockLowermost - (0.6 * self.sprite.size.height)
			self.veloc.y = 0
			
			
		} else if insideFromRight && ((inFirstQuadrant && insideFromAbove) || (inFourthQuadrant && insideFromBelow)) {
			self.sprite.position.x = blockRightmost + (0.6 * self.sprite.size.width)
			self.veloc.x = 0
			
			
		} else if insideFromLeft && ((inSecondQuadrant && insideFromAbove) || (inThirdQuadrant && insideFromBelow)) {
			self.sprite.position.x = blockLeftmost - (0.6 * self.sprite.size.width)
			self.veloc.x = 0
			
		}
		
		if block == lastGroundObject && ((inFirstQuadrant && !insideFromRight) || (inSecondQuadrant && !insideFromLeft)) {
			isGrounded = false
		}
		*/
		
	}
	
	func accelerateX(by magnitude: Double) {
		if isGrounded {
			self.veloc.x += magnitude
			
		// Apply a penalty for changing direction midair
		} else {
			let reduction = (self.moveSpeed.x - FairResist) * 0.7

			// reduces magnitude of "magnitude" by "reduction" - sign agnostic
			self.veloc.x += magnitude - (magnitude/abs(magnitude)) * reduction
			
		}
		// Check for horizontal maxVeloc - i.e. max run speed
		
		if abs(self.veloc.x) > self.maxVeloc.x {
			// sets magnitude of "self.veloc.x" to "self.maxVeloc.x" - sign agnostic
			self.veloc.x = self.veloc.x/abs(self.veloc.x) * self.maxVeloc.x
			
		}
		
	}
	
	func accelerateY(by magnitude: Double) {
		
		// For jumping
		if magnitude > 0 && isGrounded {
			self.veloc.y += magnitude
			isJumping = true
			isGrounded = false

		// For gravity
		} else if magnitude < 0 {
			self.veloc.y += magnitude
		}
		
		// Check for vertical maxVeloc - i.e. terminal velocity from gravity
		if self.veloc.y < self.maxVeloc.y {
			self.veloc.y = self.maxVeloc.y
		}
	}
	
	// This will stop object at 0 - therefore not "deceleration"
	
	func decreaseMagnitude(_ velocity: inout Double, by magnitude: Double) {
		
		// Do not use with a negative value, magnitude does not have a sign
		if magnitude > 0 {
			
			// stop object if magnitude of current velocity is less than the value to reduce by
			// this prevents acceleration in the opposite direction
			
			// NOTE: Velocity may skip directly from positive to negative between frames so we cannot check for zero velocity
			
			if abs(velocity) < magnitude {
				velocity = 0
			
			// decrease magnitude using a sign-agnostic method
			} else {
				velocity -= velocity/abs(velocity) * magnitude
				
			}
		}
	}
	
	// Apply air resistance, gravity and friction
	func applyConstantForces() {
		
		if isGrounded {
			
			// Apply friction
			decreaseMagnitude(&self.veloc.x, by: Ffriction)
			
		} else {
			
			// Apply gravity
			accelerateY(by: -Fgravity)

			// Apply air resistance
			decreaseMagnitude(&self.veloc.x, by: FairResist)
			
		}
	}
	
	init(sprite: SKSpriteNode) {
		self.sprite = sprite
	}
}
