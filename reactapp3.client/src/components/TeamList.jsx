import { useState, useEffect } from 'react';
import { Link } from 'react-router-dom';
import api from '../utils/api';
import './TeamList.css'; // Import CSS cho component

function TeamList() {
    const [teams, setTeams] = useState([]);

    useEffect(() => {
        fetchTeams();
    }, []);

    const fetchTeams = async () => {
        try {
            const response = await api.get('/team');
            setTeams(response.data);
        } catch (error) {
            console.error('Error fetching teams:', error);
        }
    };

    const handleDelete = async (id) => {
        try {
            await api.delete(`/team/${id}`);
            alert('Team deleted successfully!');
            fetchTeams();
        } catch (error) {
            console.error('Error deleting team:', error);
            alert('Failed to delete team.');
        }
    };

    return (
        <div className="team-list-container">
            <h1>Team List</h1>
            <Link to="/create-team" className="create-button">Create New Team</Link>
            <ul className="team-list">
                {teams.map((team) => (
                    <li key={team.id} className="team-item">
                        <div className="team-info">
                            <Link to={`/teams/${team.id}`} className="team-name">
                                {team.name}
                            </Link>
                            <p>Manager ID: {team.managerId}</p>
                        </div>
                        <button
                            className="delete-button"
                            onClick={() => handleDelete(team.id)}
                        >
                            Delete
                        </button>
                    </li>
                ))}
            </ul>
        </div>
    );
}

export default TeamList;
