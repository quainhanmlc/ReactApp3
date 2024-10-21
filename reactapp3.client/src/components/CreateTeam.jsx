import { useState } from 'react';
import { useNavigate } from 'react-router-dom';
import api from '../utils/api'; // Import API connector

function CreateTeam() {
    const [name, setName] = useState('');
    const [description, setDescription] = useState('');
    const [managerId, setManagerId] = useState('');
    const navigate = useNavigate();

    const handleCreate = async (e) => {
        e.preventDefault();

        const data = {
            name: name.trim(),
            description: description.trim(),
            numberOfMember: 0, // M?c ??nh là 0 khi t?o m?i
            managerId: managerId.trim(),
        };

        try {
            const response = await api.post('/team', data); // G?i API t?o team
            alert(response.data.message);
            navigate('/'); // Chuy?n h??ng v? trang danh sách
        } catch (error) {
            console.error('Error creating team:', error);
            alert('Failed to create team. Please check your input.');
        }
    };

    return (
        <div className="form-container">
            <h1>Create New Team</h1>
            <form onSubmit={handleCreate}>
                <input
                    type="text"
                    placeholder="Team Name"
                    value={name}
                    onChange={(e) => setName(e.target.value)}
                    required
                />
                <input
                    type="text"
                    placeholder="Description"
                    value={name}
                    onChange={(e) => setDescription(e.target.value)}
                    required
                />
                <input
                    type="text"
                    placeholder="Manager ID"
                    value={managerId}
                    onChange={(e) => setManagerId(e.target.value)}
                    required
                />
                <button type="submit">Create Team</button>
            </form>
        </div>
    );
}

export default CreateTeam;
