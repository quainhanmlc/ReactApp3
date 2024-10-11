import { useState } from 'react';
import api from '../utils/api'; // Import API

function ForgotPassword() {
    const [email, setEmail] = useState('');

    const handleForgotPassword = async (e) => {
        e.preventDefault();
        try {
            await api.post('/auth/forgot-password', { email });
            alert('Password reset link has been sent to your email');
        } catch (error) {
            console.error("Error sending reset link", error);
        }
    };

    return (
        <div>
            <h1>Forgot Password</h1>
            <form onSubmit={handleForgotPassword}>
                <input
                    type="email"
                    placeholder="Enter your email"
                    value={email}
                    onChange={(e) => setEmail(e.target.value)}
                    required
                />
                <button type="submit">Submit</button>
            </form>
        </div>
    );
}

export default ForgotPassword;
