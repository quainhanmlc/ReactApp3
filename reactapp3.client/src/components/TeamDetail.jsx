import { useState, useEffect } from 'react';
import { useParams, useNavigate } from 'react-router-dom';
import api from '../utils/api';
import './TeamDetail.css'; // Import CSS cho component

function TeamDetail() {
    const { id } = useParams();
    const navigate = useNavigate();
    const [team, setTeam] = useState(null);

    useEffect(() => {
        fetchTeam();
    }, []);

    const fetchTeam = async () => {
        try {
            const response = await api.get(`/team/${id}`);
            setTeam(response.data);
        } catch (error) {
            console.error('Error fetching team:', error);
        }
    };

    const handleUpdate = async () => {
        try {
            await api.put(`/team/${id}`, team);
            alert('Team updated successfully!');
            navigate('/');
        } catch (error) {
            console.error('Error updating team:', error);
            alert('Failed to update team.');
        }
    };

    if (!team) return <div>Loading...</div>;

    return (
        <div className="team-detail-container">
            <h1>Team Detail</h1>
            <form className="team-form" onSubmit={(e) => { e.preventDefault(); handleUpdate(); }}>
                <input
                    type="text"
                    value={team.name}
                    onChange={(e) => setTeam({ ...team, name: e.target.value })}
                    placeholder="Team Name"
                    className="input-field"
                    required
                />
                <textarea
                    value={team.description}
                    onChange={(e) => setTeam({ ...team, description: e.target.value })}
                    placeholder="Description"
                    className="input-field"
                    rows="4"
                />
                <input
                    type="text"
                    value={team.managerId}
                    onChange={(e) => setTeam({ ...team, managerId: e.target.value })}
                    placeholder="Manager ID"
                    className="input-field"
                    required
                />
                <button type="submit" className="submit-button">Update Team</button>
            </form>
        </div>
    );
}

export default TeamDetail;
