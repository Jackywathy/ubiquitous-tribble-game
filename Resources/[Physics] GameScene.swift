//
//  GameScene.swift
//  My Game
//
//  Created by Vincent Liu on 26/7/17.
//  Copyright © 2017 Vincent Liu. All rights reserved.
//

import SpriteKit
import GameplayKit

class GameScene: SKScene {
    
    private var playerSprite: SKSpriteNode?
	private var displayX: SKLabelNode?
	private var displayY: SKLabelNode?
	private var displayCoords: SKLabelNode?
	private var collisionBlock1: SKSpriteNode?
	private var groundObject: SKSpriteNode?
	
	var isJumping = false
	var isMovingL = false
	var isMovingR = false
	
	var frameCounter = 0
	
	var player: Player?
    
    override func didMove(to view: SKView) {
        
        // Get label node from scene and store it for use later
        self.playerSprite = self.childNode(withName: "playerSprite") as? SKSpriteNode
		self.collisionBlock1 = self.childNode(withName: "collisionBlock1") as? SKSpriteNode
		self.groundObject = self.childNode(withName: "groundObject") as? SKSpriteNode
		
		self.displayX = self.childNode(withName: "displayX") as? SKLabelNode
		self.displayY = self.childNode(withName: "displayY") as? SKLabelNode
		self.displayCoords = self.childNode(withName: "displayCoords") as? SKLabelNode
		
		displayX?.horizontalAlignmentMode = .left
		displayY?.horizontalAlignmentMode = .left
		displayCoords?.horizontalAlignmentMode = .left
		
		player = Player(sprite: playerSprite!)
	    
		// Releases controls when app moves out of focus
		NotificationCenter.default.addObserver(forName: Notification.Name.NSApplicationWillResignActive, object: nil, queue: nil, using: { _ in
			self.isJumping = false
			self.isMovingL = false
			self.isMovingR = false
		})
	}
    
    override func keyDown(with event: NSEvent) {
        switch event.keyCode {
		case 126, 13: isJumping = true
		case 123,  0: isMovingL = true
		case 124,  2: isMovingR = true
        default: break
        }
    }
	
	override func keyUp(with event: NSEvent) {
		switch event.keyCode {
		case 126, 13: isJumping = false
		case 123,  0: isMovingL = false
		case 124,  2: isMovingR = false
		default: break
		}
	}
	
    override func update(_ currentTime: TimeInterval) {
        // Called before each frame is rendered
		
		if isMovingL {
			player!.accelerateX(by: -player!.moveSpeed.x)
		}
		
		if isMovingR {
			player!.accelerateX(by: player!.moveSpeed.x)
		}
		
		if isJumping {
			player!.accelerateY(by: player!.moveSpeed.y)
			
			// Optional - requires you to repress jump key to jump again instead of holding it to autojump
			isJumping = false
		}
		
		player!.doCollisionCheck(groundObject!)
		player!.doCollisionCheck(collisionBlock1!)
		player!.applyConstantForces()
		player!.sprite.run(SKAction.moveBy(x: CGFloat(player!.veloc.x), y: CGFloat(player!.veloc.y), duration: 0.00001))
		
		frameCounter += 1
		
		if frameCounter % 5 == 0 {
			displayX!.text = "x veloc: \(player!.veloc.x)"
			displayY!.text = "y veloc: \(player!.veloc.y)"
			displayCoords!.text = "xpos: \(playerSprite!.position.x), ypos: \(playerSprite!.position.y))"
		}
    }
}
