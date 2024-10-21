import { useState } from 'react';
import { useNavigate } from 'react-router-dom';
import api from '../utils/api'; // API connector

function CreateProject() {
    const [projectName, setProjectName] = useState('');
    const [description, setDescription] = useState('');
    const [expectedEndDate, setExpectedEndDate] = useState('');
    const [customerId, setCustomerId] = useState('');
    const [image, setImage] = useState('');
    const navigate = useNavigate();

    const handleCreateProject = async (e) => {
        e.preventDefault();
        try {
            const employeeId = "64b7a8f1e16e4f31b5a5a7f9"; // ID nhân viên
            const projectStatus = "in-progress"; // Trạng thái mặc định
            const startDate = new Date().toISOString(); // Ngày bắt đầu hiện tại

            const data = {
                projectName,
                requestDescriptionFromCustomer: description,
                startDate,
                image,
                employeeId,
                expectedEndDate,
                customerId,
                projectStatus,
            };

            await api.post('/employee/create-project', data);

            alert('Project created successfully!');
            navigate('/'); // Điều hướng về trang danh sách project hoặc trang chính
        } catch (error) {
            console.error('Error creating project:', error);
            alert('Only employees in the Sales team can create projects.');
        }
    };

    return (
        <div className="form-container">
            <h1>Create Project</h1>
            <form onSubmit={handleCreateProject}>
                <input
                    type="text"
                    placeholder="Project Name"
                    value={projectName}
                    onChange={(e) => setProjectName(e.target.value)}
                    required
                />
                <textarea
                    placeholder="Request Description"
                    value={description}
                    onChange={(e) => setDescription(e.target.value)}
                    required
                />
                <input
                    type="text"
                    placeholder="Image URL"
                    value={image}
                    onChange={(e) => setImage(e.target.value)}
                />
                <input
                    type="date"
                    value={expectedEndDate}
                    onChange={(e) => setExpectedEndDate(e.target.value)}
                    required
                />
                <input
                    type="text"
                    placeholder="Customer ID"
                    value={customerId}
                    onChange={(e) => setCustomerId(e.target.value)}
                    required
                />
                <button type="submit">Create Project</button>
            </form>
        </div>
    );
}

export default CreateProject;
