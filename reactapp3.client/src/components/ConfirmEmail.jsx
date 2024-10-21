import { useState } from 'react';
import { useParams, useNavigate } from 'react-router-dom';
import api from '../utils/api';

function ConfirmEmail() {
    const { email } = useParams(); // Lấy email từ URL
    const [code, setCode] = useState('');
    const navigate = useNavigate();

    const handleConfirmEmail = async (e) => {
        e.preventDefault();
        try {
            await api.post('/auth/confirm-email', { email, code });
            alert('Email confirmed! Please log in.');
            navigate('/login'); // Điều hướng về trang đăng nhập
        } catch (error) {
            console.error('Email confirmation failed', error);
            alert('Invalid or expired confirmation code.');
        }
    };

    return (
        <div className="form-container">
            <h1>Confirm Email</h1>
            <form onSubmit={handleConfirmEmail}>
                <input
                    type="text"
                    placeholder="Enter confirmation code"
                    value={code}
                    onChange={(e) => setCode(e.target.value)}
                    required
                />
                <button type="submit">Confirm Email</button>
            </form>
        </div>
    );
}

export default ConfirmEmail;
