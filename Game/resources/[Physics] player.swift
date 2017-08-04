//
//  player.swift
//  My Game
//
//  Created by Vincent Liu on 27/7/17.
//  Copyright © 2017 Vincent Liu. All rights reserved.
//

import Foundation
import SpriteKit

let Fgravity = 0.5
let Ffriction = 1.0
let FairResist = 3.5

struct Velocity {
	var x: Double
	var y: Double
}

class Player {
	var sprite: SKSpriteNode
	
	let moveSpeed = Velocity(x:  4.0, y: 17.0) // in pixels per frame
	let maxVeloc  = Velocity(x: 10.0, y: -8.0)
	var veloc     = Velocity(x:  0.0, y:  0.0)
	
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
		
		
		/* 
		   |N |
		 __|  |__
		 W  XX  E
		 __ XX __
		   |  |
		   |S |
		*/
		
		// NOTE: N and S have priority on purpose
		
		// N
		
		if selfCentre.y > blockUppermost && insideFromAbove && insideFromLeft && insideFromRight {
			self.sprite.position.y = blockUppermost + (0.5 * self.sprite.size.height)
			if self.veloc.y < 0 { self.veloc.y = 0 }
			isGrounded = true
			lastGroundObject = block
			print("\(block.name!): NORTH")
		}
			
		// S
			
		else if selfCentre.y < blockLowermost && insideFromBelow && insideFromLeft && insideFromRight {
			
			// Don't do anything if player is already falling from gravity (wallhugging), otherwise (jumping directly under block) remove their velocity
			if self.veloc.y > 0 {
				self.sprite.position.y = blockLowermost - (0.5 * self.sprite.size.height)
				self.veloc.y = 0
			}
			print("\(block.name!): SOUTH")
			
		}
		
		// W
		
		else if selfCentre.x < blockLeftmost && insideFromLeft && insideFromAbove && insideFromBelow {
			self.sprite.position.x = blockLeftmost - (0.5 * self.sprite.size.width)
			self.veloc.x = 0
			print("\(block.name!): WEST")
		}
		
		// E
		
		else if selfCentre.x > blockRightmost && insideFromRight && insideFromAbove && insideFromBelow {
			self.sprite.position.x = blockRightmost + (0.5 * self.sprite.size.width)
			self.veloc.x = 0
			print("\(block.name!): EAST")
		}
			
		 else if block == lastGroundObject && !(insideFromLeft && insideFromRight) {
			isGrounded = false
			
		}
		
		// Quadrant implementation
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
		self.veloc.x += magnitude
	}
	
	func accelerateY(by magnitude: Double) {
		if isGrounded {
			self.veloc.y += magnitude
			isJumping = true
			
			if magnitude != 0 {
				isGrounded = false
			}
		}
	}
	
	// designed to stop object at 0 - not technically "deceleration"
	func decelerate(_ velocity: inout Double, by magnitude: Double) {
		
		// if magnitude of velocity is less than deceleration then stop object
		if abs(velocity) < magnitude {
			velocity = 0
			
		} else {
			
			// decrease velocity magnitude, do nothing if veloc = 0
			if velocity > 0 {
				velocity -= magnitude
			} else if velocity < 0 {
				velocity += magnitude
			}
		}
	}
	
	func updateVeloc() {
		
		// =================================
		// Check for maxVeloc exceedance
		// =================================
		
		// horizontal - max run speed
		
		if abs(self.veloc.x) > self.maxVeloc.x {
			self.veloc.x = self.veloc.x > 0 ? self.maxVeloc.x : -self.maxVeloc.x
		}
		
		// vertical - terminal velocity
		
		if self.veloc.y < self.maxVeloc.y {
			self.veloc.y = self.maxVeloc.y
		}
		
		// ==================================
		// Apply air resistance, gravity and friction
		// ==================================
		
		if isGrounded {
			
			self.veloc.y = 0
			
			// apply friction
			decelerate(&self.veloc.x, by: Ffriction)
			
		} else {
			
			// apply gravity
			self.veloc.y -= Fgravity

			// apply air resistance
			if self.veloc.x != 0 {
				decelerate(&self.veloc.x, by: FairResist)
			}
		}
		
		self.sprite.run(SKAction.moveBy(x: CGFloat(self.veloc.x), y: CGFloat(self.veloc.y), duration: 0.00001))
	}
	
	init(sprite: SKSpriteNode) {
		self.sprite = sprite
	}
}
