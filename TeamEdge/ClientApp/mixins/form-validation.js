export default{
    methods:{
        validate () {
            if (this.$refs.form.validate()) {
              this.snackbar = true
            }
          },
          reset () {
            this.$refs.form.reset()
          },
          resetValidation () {
            this.$refs.form.resetValidation()
          }
    }
}