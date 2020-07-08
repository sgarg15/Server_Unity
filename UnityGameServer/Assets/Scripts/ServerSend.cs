﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ServerSend
{
	private static void SendTCPData(int _toClient, Packet _packet)
	{
		_packet.WriteLength();
		Server.clients[_toClient].tcp.SendData(_packet);
	}

	private static void SendUDPData(int _toClient, Packet _packet)
	{
		_packet.WriteLength();
		Server.clients[_toClient].udp.SendData(_packet);
	}

	private static void SendTCPDataToAll(Packet _packet)
	{
		_packet.WriteLength();
		for (int i = 1; i <= Server.MaxPlayers; i++)
		{
			Server.clients[i].tcp.SendData(_packet);
		}
	}
	private static void SendTCPDataToAll(int _exceptClient, Packet _packet)
	{
		_packet.WriteLength();
		for (int i = 1; i <= Server.MaxPlayers; i++)
		{
			if (i != _exceptClient)
			{
				Server.clients[i].tcp.SendData(_packet);
			}
		}
	}

	private static void SendUDPDataToAll(Packet _packet)
	{
		_packet.WriteLength();
		for (int i = 1; i <= Server.MaxPlayers; i++)
		{
			Server.clients[i].udp.SendData(_packet);
		}
	}
	private static void SendUDPDataToAll(int _exceptClient, Packet _packet)
	{
		_packet.WriteLength();
		for (int i = 1; i <= Server.MaxPlayers; i++)
		{
			if (i != _exceptClient)
			{
				Server.clients[i].udp.SendData(_packet);
			}
		}
	}

	#region Packets
	public static void Welcome(int _toClient, string _msg)
	{
		using (Packet _packet = new Packet((int)ServerPackets.welcome))
		{
			_packet.Write(_msg);
			_packet.Write(_toClient);

			SendTCPData(_toClient, _packet);
		}
	}

	public static void SpawnPlayer(int _toClient, Player _player)
	{
		using (Packet _packet = new Packet((int)ServerPackets.spawnPlayer))
		{
			_packet.Write(_player.id);
			_packet.Write(_player.username);
			_packet.Write(_player.transform.position);
			_packet.Write(_player.transform.rotation);

			SendTCPData(_toClient, _packet);
		}
	}

	public static void PlayerPosition(Player _player)
	{
		using (Packet _packet = new Packet((int)ServerPackets.playerPosition))
		{
			_packet.Write(_player.id);
			_packet.Write(_player.transform.position);

			SendUDPDataToAll(_packet);
		}
	}

	public static void PlayerRotation(Player _player)
	{
		using (Packet _packet = new Packet((int)ServerPackets.playerRotation))
		{
			_packet.Write(_player.id);
			_packet.Write(_player.transform.rotation);

			SendUDPDataToAll(_player.id, _packet);
		}
	}

	public static void PlayerDisconnected(int _playerId)
    {
        using (Packet _packet = new Packet((int)ServerPackets.playerDisconnected))
        {
			_packet.Write(_playerId);
			SendTCPDataToAll(_packet);
        }
    }

	public static void PlayerHealth(Player _player)
    {
        using (Packet _packet = new Packet((int) ServerPackets.playerHealth))
        {
			_packet.Write(_player.id);
			_packet.Write(_player.health);

			SendTCPDataToAll(_packet);
        }
    }

	public static void PlayerRespawned(Player _player)
	{
		using (Packet _packet = new Packet((int)ServerPackets.playerRespawned))
		{
			_packet.Write(_player.id);

			SendTCPDataToAll(_packet);
		}
	}

	public static void CreateItemSpawner(int _toClient, int _spawnerId, Vector3 _spawnerPosition, bool _hasItem)
    {
        using (Packet _packet = new Packet((int)ServerPackets.createItemSpawner))
        {
			_packet.Write(_spawnerId);
			_packet.Write(_spawnerPosition);
			_packet.Write(_hasItem);

			SendTCPData(_toClient, _packet);
        }
    }

	public static void ItemSpawned(int _spawnerId)
    {
		using (Packet _packet = new Packet((int)ServerPackets.itemSpawned))
        {
			_packet.Write(_spawnerId);

			SendTCPDataToAll(_packet);
        }
    }

	public static void ItemPickedUp(int _spawnerId, int _byPlayer)
	{
		using (Packet _packet = new Packet((int)ServerPackets.itemPickedUp))
		{
			_packet.Write(_spawnerId);
			_packet.Write(_byPlayer);

			SendTCPDataToAll(_packet);
		}
	}

	public static void SpawnProjectile(Projectile _projectile, int _thrownByPlayer)
    {
		using (Packet _packet = new Packet((int)ServerPackets.spawnProjectile))
        {
			_packet.Write(_projectile.id);
			_packet.Write(_projectile.transform.position);
			_packet.Write(_thrownByPlayer);

			SendTCPDataToAll(_packet);
        }
    }

	public static void ProjectilePosition(Projectile _projectile)
	{
		using (Packet _packet = new Packet((int)ServerPackets.projectilePosition))
		{
			_packet.Write(_projectile.id);
			_packet.Write(_projectile.transform.position);

			SendTCPDataToAll(_packet);
		}
	}

	public static void ProjectileExploded(Projectile _projectile)
	{
		using (Packet _packet = new Packet((int)ServerPackets.projectileExploded))
		{
			_packet.Write(_projectile.id);
			_packet.Write(_projectile.transform.position);

			SendTCPDataToAll(_packet);
		}
	}
	#endregion
}
