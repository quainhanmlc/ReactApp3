import { useState } from 'react';
import { useNavigate } from 'react-router-dom';
import api from '../utils/api'; // Cần import API bạn đã cấu hình
import '../App.css';

function Login() {
    const [username, setUsername] = useState('');
    const [password, setPassword] = useState('');
    const navigate = useNavigate();

    const handleLogin = async (e) => {
        e.preventDefault();
        try {
            const response = await api.post('/auth/login', { username, password });
            localStorage.setItem('token', response.data.token);
            navigate('/'); // Điều hướng đến trang chính sau khi đăng nhập
        } catch (error) {
            console.error("Login failed", error);
        }
    };

    return (
        <div className="container">
            <h1>Login Page</h1>
            <form onSubmit={handleLogin}>
                <input
                    type="text"
                    placeholder="Username"
                    value={username}
                    onChange={(e) => setUsername(e.target.value)}
                    required
                />
                <input
                    type="password"
                    placeholder="Password"
                    value={password}
                    onChange={(e) => setPassword(e.target.value)}
                    required
                />
                <button type="submit">Login</button>
            </form>
            <button className="link" onClick={() => navigate('/forgot-password')}>Forgot Password?</button>
            <button className="link" onClick={() => navigate('/register')}>Register</button>
        </div>
    );
}

export default Login;
