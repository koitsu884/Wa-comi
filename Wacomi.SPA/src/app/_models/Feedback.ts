export interface Feedback {
    senderId? : number;
    senderName : string;
    email : string;
    title : string;
    content : string;
}

// public int Id { get; set;}
//         public int? SenderId { get; set;}
//         public AppUser Sender { get; set;}
//         [Required]
//         [MaxLength(100)]
//         public string SenderName { get; set;}
//         [Required]
//         public string Email { get; set;}
//         [MaxLength(100)]
//         [Required]
//         public string Title{ get; set;}
//         [Required]
//         public string Content { get; set;}